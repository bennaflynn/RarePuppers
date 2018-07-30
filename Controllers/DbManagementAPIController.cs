using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RarePuppers.Data;
using RarePuppers.Models.ViewModels.DbItems;
using RarePuppers.Models.ViewModels.NewFolder;

namespace RarePuppers.Controllers
{
    [Produces("application/json")]
    [Route("api/DbManagementAPI")]
    public class DbManagementAPIController : Controller
    {
        ApplicationDbContext context;
        //TO DO: Add validation to ensure that user is an admin
        public DbManagementAPIController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("AddNewAttributeType")]
        public JSONResponseVM AddNewPupperAttributeType([FromBody] AttributeTypeVM attr)
        {
            //TO DO: check to see if the user is an admin

            //does this attribute exists?
            var query = context.AttributeTypes.Where(a => a.attribute_type_id == attr.attribute_type_id).FirstOrDefault();

            if(query != null)
            {
                return new JSONResponseVM { success = false, message = "An attribute with this ID already exists" };
            }

            AttributeType att = new AttributeType
            {
                attribute_type_id = attr.attribute_type_id,
                name = attr.name
            };
            context.AttributeTypes.Add(att);
            context.SaveChanges();

            return new JSONResponseVM { success = true, message = "Successfully add new attribute: " + att.name };
        }
    }
}