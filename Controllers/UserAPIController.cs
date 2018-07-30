using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RarePuppers.Data;
using RarePuppers.Models.ViewModels;
using RarePuppers.Models.ViewModels.JSONResponse;
using RarePuppers.Models.ViewModels.NewFolder;
using RarePuppers.Services;

namespace RarePuppers.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserAPIController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration config;

        public UserAPIController(IConfiguration config, ApplicationDbContext context)
        {
            this.context = context;
            this.config = config;
        }

        //TO DO: Add a route to go after the user has created an account
        [HttpPost]
        [Route("CreateAccount")]
        public async Task<JsonResult> CreateAccount([FromBody] CreateAccountVM account )
        {
            //validate

            if (account.username == null || account.password == null || account.password2 == null)
            {
                return Json(new JSONResponseVM { success= false, message= "Please fill out all the fields" });
            }
            if(account.password != account.password2)
            {
                return Json(new JSONResponseVM {success= false, message= "Passwords don't match"});
            }

            //check to see if this username already exists
            var query = await context.Users.Where(u => u.username == account.username).FirstOrDefaultAsync();
            if (query != null)
            {
                //this username exists
                return Json(new JSONResponseVM { success= false, message= "A user with this username already exists" });
            }

            
            string newPassword = account.password;
            newPassword = HashString.Hash(newPassword, config["salt"]);

            //now add the new user to the database
            User newUser = new User
            {
                username = account.username,
                hashedPassword = newPassword,
                //the user is by default a customer
                role_id = 0,
                role = await context.Roles.Where(r => r.role_id == 0).FirstOrDefaultAsync()
            };
            context.Users.Add(newUser);
            context.SaveChanges();
            return Json(new JSONTokenResponseVM { message="Successfully created account: " + newUser.username, token = "dashfkjadfasdf"});
        }

        [HttpPost]
        [Route("Login")]
        public async Task<JsonResult> Login([FromBody] LoginVM loginUser)
        {
            //hash the password
            string password = HashString.Hash(loginUser.password, config["salt"]);

            var user = await context.Users
                .Where(u => u.username == loginUser.username)
                .Where(u => u.hashedPassword == password)
                .FirstOrDefaultAsync();

            if(user != null)
            {
                return Json(new JSONTokenResponseVM { message= "Successfully logged in", token = "hdjkahdajksdhafk" } );
            } else
            {
                return Json(new JSONResponseVM { success = false, message = "Incorrect login details"});
            }
        }
    }
}