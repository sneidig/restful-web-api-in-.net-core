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
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

    }
}
