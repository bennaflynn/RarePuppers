using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RarePuppers.Models;

namespace RarePuppers.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    //the user table
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string username { get; set; }
        public string hashedPassword { get; set; }

        //link the children
        public ICollection<Pupper> puppers { get; set; }
    }

    //the pupper table
    public class Pupper
    {
        [Key, Column(Order = 0)]
        public int pupper_id { get; set; }
        [Key, Column(Order = 1)]
        public int user_id { get; set; }
        public int tail { get; set; }
        public int ears { get; set; }
        public int color { get; set; }
        public int eyes { get; set; }

        //now reference the parent tables
        public virtual User User { get; set; }
    }

    //the pupper attributes table
    public class Attribute
    {
        [Key]
        public int attribute_id { get; set; }
        public string name { get; set; }
        public decimal rarity { get; set; }
        public string image_src { get; set; }

    }
}
