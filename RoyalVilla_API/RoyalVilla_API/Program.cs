using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext with IoC container
//  ApplicationDbContext is the class that builds DbContext
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(defaultConnection))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(defaultConnection);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(o => {
    // Map Villa to VillaCreatedDTO
    // using RevereMap will cover both possible source and destination mappings
    o.CreateMap<Villa, VillaCreateDTO>().ReverseMap();
    o.CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
    o.CreateMap<Villa, VillaDTO>().ReverseMap();
});

var app = builder.Build();

// Call the seed data migration method 
await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // From Section 2: Introduction
    //  Add Scalar
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// Setup the method SeeedDataAsync to run automatically when the app starts
//  This method calls MigrateAsync() so there is no need to manually run update-database
static async Task SeedDataAsync(WebApplication app)
{
    // create a scope
    // using will dispose of the scoped instance once MigrateAsync is done
    using var scope = app.Services.CreateScope();

    // get access to the dbContext
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // setup MigrateAsync to automatically apply any pending migrations
    await context.Database.MigrateAsync();
}