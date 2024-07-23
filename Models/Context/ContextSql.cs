using Entities.Core;
using Entities.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Context
{
    public class ContextSql : IdentityDbContext<ApplicationUser>
    {
        public ContextSql(DbContextOptions<ContextSql> options)
            : base(options)
        {
        }

        //public DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roleAdmin = new IdentityRole()
            {
                Id = "bfbdd3a1-0bea-40cd-b9c5-5f76e4c9bc3f",
                Name = "administrador",
                NormalizedName = "administrador"
            };

            builder.Entity<IdentityRole>().HasData(roleAdmin);
            base.OnModelCreating(builder);
        }
    }
}
