﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.DAL.Context;

public class WebStoreDB : IdentityDbContext<User, Role, string>
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Section> Sections { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public WebStoreDB(DbContextOptions<WebStoreDB> options) : base(options) { }
}