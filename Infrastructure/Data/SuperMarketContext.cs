using DB_Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SuperMarketContext : IdentityDbContext<User>
    {
        public SuperMarketContext(DbContextOptions<SuperMarketContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "User",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "User".ToUpper(),
            }, new IdentityRole
            {
                Name = "Admin",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "Admin".ToUpper(),
            });
            builder.Entity<User>().ToTable("Users");
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
   
}
