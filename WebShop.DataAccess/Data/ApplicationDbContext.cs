using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebShop.Models.Models;

namespace WebShop.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    // Always use plural here
    public DbSet<Category> Categories { get; set; }

    // Seed data here (there are other ways of doing this)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new
            {
                Id = 1,
                Name = "Action",
                DisplayOrder = 1
            },
            new
            {
                Id = 2,
                Name = "Thriller",
                DisplayOrder = 2
            },
            new
            {
                Id = 3,
                Name = "SciFi",
                DisplayOrder = 3
            },
            new
            {
                Id = 4,
                Name = "Romance",
                DisplayOrder = 4
            });
    }
}
