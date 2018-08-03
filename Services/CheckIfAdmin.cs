using RarePuppers.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace RarePuppers.Services
{
    public static class CheckIfAdmin
    {
        public static bool IsAdmin(string username, ApplicationDbContext db)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();
            ////read the boy
            //var tokenStr = tokenHandler.ReadJwtToken(token) as JwtSecurityToken;
            ////the claim type of sub is the username
            //var username = tokenStr.Claims.First(claim => claim.Type == "sub").Value;
            ////get the role id from the user table
            var userRoleId = db.Users.Where(u => u.username == username).Select(u => u.role_id).FirstOrDefault();
            //is this user an admin?
            return userRoleId == 1 ? true : false;
        }
    }
}
