using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoAPI.Models;
using DemoAPI.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DemoAPI.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AdminCredentials _credentials;
        private readonly AuthOptions _options;

        public AccountController(AdminCredentials credentials, AuthOptions options)
        {
            _credentials = credentials;
            _options = options;
        }

        [HttpPost("/login")]
        public ActionResult<AuthResponse> GetToken(User user)
        {
            var identity = GetIdentity(user);
            if (identity == null)
                return BadRequest();

            var jwt = new JwtSecurityToken(
                claims: identity.Claims, expires: DateTime.UtcNow.AddMinutes(_options.ExpiresIn),
                signingCredentials: new SigningCredentials(_options.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256Signature));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var expiration = DateTimeOffset.UtcNow.AddMinutes(_options.ExpiresIn);

            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", encodedJwt,
                new CookieOptions
                {
                    Domain = null,
                    HttpOnly = true,
                    Expires = expiration
                });

            return new AuthResponse(encodedJwt, expiration);
        }

        private ClaimsIdentity? GetIdentity(User user)
        {
            if (user.Login == _credentials.Login && user.Password == _credentials.Password)
            {
                var claims = new List<Claim> {new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)};
                return new ClaimsIdentity(claims, "Token");
            }

            return null;
        }
    }
}