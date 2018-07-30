using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RarePuppers.Data
{
    public class Seeder
    {
        private ApplicationDbContext context;
        public Seeder(ApplicationDbContext _context)
        {
            context = _context;
        }
        //populate the database with the static items
        public void SeedData()
        {
            //jump out if data already exists
            if(context.Roles.Count() != 0)
            {
                return;
            }

            //build the home types
            HomeType basic = new HomeType
            {
                home_type_id = 0,
                name = "Town House",
                capacity = 2,
                home_image_src = "changeThisLater"
            };
            context.HomeTypes.Add(basic);

            HomeType premium = new HomeType
            {
                home_type_id = 1,
                name = "Home",
                capacity = 3,
                home_image_src = "ChangeThisLater"
            };
            context.HomeTypes.Add(premium);

            HomeType deluxe = new HomeType {
                home_type_id = 2,
                name = "Farm House",
                capacity = 5,
                home_image_src = "ChangeThisLater"
            };
            context.HomeTypes.Add(deluxe);

            context.SaveChanges();

            Role user = new Role
            {
                role_id = 0,
                role_name = "User"
            };
            context.Roles.Add(user);

            Role admin = new Role
            {
                role_id = 1,
                role_name = "Admin"
            };
            context.Roles.Add(admin);

            //save the seeded database
            context.SaveChanges();
        }
    }
}
