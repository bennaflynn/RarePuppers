﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        //TO DO: Role management, including admin verification

        
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

            //generate the token
            GenerateJWT jwtGen = new GenerateJWT();
            TokenVM tok = jwtGen.Generate(newUser.username, config);
            return Json(new JSONTokenResponseVM { message="Successfully created account: " + newUser.username, token = tok.token});
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
                //Generate the JWT
                GenerateJWT jwtGen = new GenerateJWT();
                TokenVM token = jwtGen.Generate(user.username, config);
                return Json(new JSONTokenResponseVM { message= "Successfully logged in", token = token.token } );
            } else
            {
                return Json(new JSONResponseVM { success = false, message = "Incorrect login details"});
            }
        }

        //TO DO: Delete this test
        [HttpGet]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [Route("Test")]
        public JsonResult Test()
        {
            return Json(new JSONResponseVM { success = true, message = "In baby" });
        }
    }
}