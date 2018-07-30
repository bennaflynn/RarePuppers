using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RarePuppers.Services
{
    public class GenerateJWT
    {
        public TokenVM Generate(string username, IConfiguration config)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenInformation:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //same signin should work for the next week
            var expires = DateTime.Now.AddDays(Convert.ToDouble(7));

            var token = new JwtSecurityToken(
                config["TokenInformation:Issuer"],
                config["TokenInformation:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
                );

            var formattedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenVM { token = formattedToken };

        }
    }

    //view model for just the token
    public class TokenVM
    {
        public string token { get; set; }
    }
}
