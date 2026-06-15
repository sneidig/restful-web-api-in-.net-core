using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Villa> Villas { get; set; }

        // Alternate modern C# syntax
        //public ApplicationDbContext(DbContextOptions options) : DbContext(options) { }

        // Create a constructor and pass options to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>()
                .Property(v => v.Rate)
                .HasPrecision(5, 2);
        }

    }
}
