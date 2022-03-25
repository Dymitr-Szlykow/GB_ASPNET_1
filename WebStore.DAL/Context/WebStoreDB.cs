using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Domain.Entities;

namespace GB.ASPNET.WebStore.DAL.Context;

public class WebStoreDB : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Section> Sections { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options) { }


    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<Brand>()
    //        .HasMany(brand => brand.Products)
    //        .WithOne(product => product.Brand)
    //        .OnDelete(DeleteBehavior.Cascade);
    //}
}