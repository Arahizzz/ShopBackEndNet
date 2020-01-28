using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DemoAPI
{
    public class CookieExtractor
    {
        private readonly RequestDelegate _next;

        public CookieExtractor(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies[".AspNetCore.Application.Id"];
            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            await _next(context);
        }
    }
}