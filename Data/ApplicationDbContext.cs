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

        //define the collections
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Pupper> Puppers { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<AttributeType> AttributeTypes { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<HomeType> HomeTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //define the composite primary keys
            builder.Entity<Pupper>()
                .HasKey(p => new { p.pupper_id, p.user_id });
            builder.Entity<Home>()
                .HasKey(h => new { h.home_id, h.user_id });

            //define foreign keys
            builder.Entity<User>()
                .HasOne(u => u.role)
                .WithMany(u => u.Users)
                .HasForeignKey(fk => new { fk.role_id })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Pupper>()
                .HasOne(p => p.User)
                .WithMany(p => p.puppers)
                .HasForeignKey(fk => new { fk.user_id })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Attribute>()
                .HasOne(a => a.attribute)
                .WithMany(a => a.attributes)
                .HasForeignKey(fk => new { fk.attribute_type_id })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Home>()
                .HasOne(h => h.HomeType)
                .WithMany(h => h.homes)
                .HasForeignKey(fk => new { fk.home_type_id })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Home>()
                .HasOne(h => h.User)
                .WithMany(h => h.homes)
                .HasForeignKey(fk => new { fk.user_id })
                .OnDelete(DeleteBehavior.Restrict);

            //prevent auto generation of the id feilds
            builder.Entity<Role>().Property(m => m.role_id).ValueGeneratedNever();
            builder.Entity<HomeType>().Property(m => m.home_type_id).ValueGeneratedNever();
            builder.Entity<AttributeType>().Property(m => m.attribute_type_id).ValueGeneratedNever();

        }
    }

    //the user table
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string username { get; set; }
        public string hashedPassword { get; set; }
        public int role_id { get; set; }

        //link the parent
        public virtual Role role { get; set; }

        //link the children
        public ICollection<Pupper> puppers { get; set; }
        public ICollection<Home> homes { get; set; }
    }

    //the roles table
    public class Role
    {
        [Key]
        public int role_id { get; set; }
        public string role_name { get; set; }

        public ICollection<User> Users { get; set; }
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
        public int attribute_type_id { get; set; }

        //refernce the parent
        public virtual AttributeType attribute { get; set; }

    }

    public class AttributeType
    {
        [Key]
        public int attribute_type_id { get; set; }
        public string name { get; set; }

        //reference the children
        public ICollection<Attribute> attributes { get; set; }
    }

    public class Home
    {
        [Key, Column(Order = 0)]
        public int home_id { get; set; }
        [Key, Column(Order = 1)]
        public int user_id { get; set; }
        public string name { get; set; }
        public int home_type_id { get; set; }

        //refernce the parents
        public virtual User User { get; set; }
        public virtual HomeType HomeType { get; set; }
    }

    public class HomeType
    {
        [Key]
        public int home_type_id { get; set; }
        public int capacity { get; set; }
        public string name { get; set; }
        public string home_image_src { get; set; }

        //reference the child
        public ICollection<Home> homes { get; set; }
    }

}
