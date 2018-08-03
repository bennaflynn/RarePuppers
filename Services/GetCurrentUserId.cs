using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace RarePuppers.Services
{
    public static class GetCurrentUsername
    {
        public static string CurrentUser(this IHttpContextAccessor httpContext)
        {
            var username = httpContext?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            return username;
        }
    }
}
