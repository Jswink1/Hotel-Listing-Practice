using HotelListingPractice.DataAccess.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingPractice.DataAccess.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        // Seed the Database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add this after implementing IdentityDbContext
            base.OnModelCreating(modelBuilder);

            // Seed Country Data
            modelBuilder.ApplyConfiguration(new CountrySeed());

            // Seed Hotel Data
            modelBuilder.ApplyConfiguration(new HotelSeed());

            // Seed Identity Roles
            modelBuilder.ApplyConfiguration(new RoleSeed());
        }
    }
}
