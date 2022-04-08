using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Orders;

namespace GB.ASPNET.WebStore.DAL.Context;

public class WebStoreDB : IdentityDbContext<User, Role, string>
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Section> Sections { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options) { }
}