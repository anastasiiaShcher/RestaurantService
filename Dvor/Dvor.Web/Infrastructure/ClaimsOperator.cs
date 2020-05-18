using System.Collections.Generic;
using System.Security.Claims;
using Dvor.Common.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Dvor.Web.Infrastructure
{
    public static class ClaimsOperator
    {
        public static ClaimsPrincipal CreatePrincipal(IEnumerable<Claim> claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }

        public static IEnumerable<Claim> GenerateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            
            return claims;
        }
    }
}