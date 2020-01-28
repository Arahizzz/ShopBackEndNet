using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoAPI.Options;
using DemoAPI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DemoAPI
{
    public class Startup
    {
        private readonly List<string> _allowedDomains = new List<string>();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<DataBaseSettings>(
                Configuration.GetSection(nameof(DataBaseSettings)));

            services.Configure<AdminCredentials>(Configuration.GetSection(nameof(AdminCredentials)));

            services.AddSingleton<IDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DataBaseSettings>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<AdminCredentials>>().Value);

            var authOptions = new AuthOptions();
            Configuration.Bind(nameof(AuthOptions), authOptions);
            services.AddSingleton(authOptions);

            Configuration.Bind("AllowedDomains", _allowedDomains);

            services.AddSingleton<Mongo>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<CategoryService>();
            services.AddSingleton<OrderService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = false,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = false,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
 
                        // установка ключа безопасности
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .WithOrigins(_allowedDomains.ToArray()) // путь к нашему SPA клиенту
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseMiddleware<CookieExtractor>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}