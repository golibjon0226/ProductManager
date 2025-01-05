using ProductManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
           .Property(p => p.Id)
           .HasConversion<long>()
           .UseIdentityColumn();


    }

}
