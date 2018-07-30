using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RarePuppers.Data;

namespace RarePuppers.Controllers
{
    [Produces("application/json")]
    [Route("api/PupperAPI")]
    public class PupperAPIController : Controller
    {
        //to handle all out pupper interactions

        ApplicationDbContext context;
        public PupperAPIController(ApplicationDbContext context)
        {
            this.context = context;
        }

        
    }
}