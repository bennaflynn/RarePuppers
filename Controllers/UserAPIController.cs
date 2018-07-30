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
                return Json("{ success: false, message: Please fill out all the fields }");
            }
            if(account.password != account.password2)
            {
                return Json("{success: false, message: Passwords don't match}");
            }

            //check to see if this username already exists
            var query = await context.Users.Where(u => u.username == account.username).FirstOrDefaultAsync();
            if (query != null)
            {
                //this username exists
                return Json("{success: false, message: A user with this username already exists}");
            }

            
            string newPassword = account.password + config["salt"];
            newPassword = HashString.Hash(newPassword);

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
            return Json("{success: true, message: created new user " + account.username + "}");
        }
    }
}