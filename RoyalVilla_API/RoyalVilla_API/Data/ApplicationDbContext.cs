using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Villa> Villa { get; set; }

        // Alternate modern C# syntax
        //public ApplicationDbContext(DbContextOptions options) : DbContext(options) { }

        // Create a constructor and pass options to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>()
                .Property(v => v.Rate)
                .HasPrecision(5, 2);

            // Define the JSON-formatted seed data in the model
            // The next time add-migration is run EF generates a migration file with 
            //  insert statemetns for this data
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Luxurious villa with stunning ocean views and private beach access.",
                    Rate = 500.0m,
                    Sqft = 2500,
                    Occupancy = 6,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                    CreatedDate = new DateTime(2024, 1, 1),
                    UpdatedDate = new DateTime(2024, 1, 1)
                },
                new Villa
                {
                    Id = 2,
                    Name = "Diamond Villa",
                    Details = "Elegant villa with marble interiors and panoramic mountain views.",
                    Rate = 750.0m,
                    Sqft = 3200,
                    Occupancy = 8,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                    CreatedDate = new DateTime(2024, 1, 15),
                    UpdatedDate = new DateTime(2024, 1, 15)
                },
                new Villa
                {
                    Id = 3,
                    Name = "Pool Villa",
                    Details = "Modern villa featuring an infinity pool and outdoor entertainment area.",
                    Rate = 350.0m,
                    Sqft = 1800,
                    Occupancy = 4,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                    CreatedDate = new DateTime(2024, 2, 1),
                    UpdatedDate = new DateTime(2024, 2, 1)
                },
                new Villa
                {
                    Id = 4,
                    Name = "Luxury Villa",
                    Details = "Premium villa with spa facilities and concierge services.",
                    Rate = 900.0m,
                    Sqft = 4000,
                    Occupancy = 10,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                    CreatedDate = new DateTime(2024, 2, 14),
                    UpdatedDate = new DateTime(2024, 2, 14)
                },
                new Villa
                {
                    Id = 5,
                    Name = "Garden Villa",
                    Details = "Charming villa surrounded by tropical gardens and nature trails.",
                    Rate = 275.0m,
                    Sqft = 1500,
                    Occupancy = 3,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg",
                    CreatedDate = new DateTime(2024, 3, 1),
                    UpdatedDate = new DateTime(2024, 3, 1)
                }
            );

        }

    }
}
