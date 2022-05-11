using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.Services;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


string? db = builder.Configuration["Database"];
_ = db switch
{
    "SqlServer" or "DockerDB"
        => builder.Services.AddDbContext<WebStoreDB>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString(db))),

    "Sqlite"
        => builder.Services.AddDbContext<WebStoreDB>(opt =>
            opt.UseSqlite(
                builder.Configuration.GetConnectionString(db),
                arg => arg.MigrationsAssembly("WebStore.DAL.Sqlite"))),

    _ => throw new ApplicationException("Ошибка чтения строки подключения к БД.")
};


builder.Services
    .AddTransient<IDbInitializer, DbInitializer>()
    .AddScoped<IEmployeesData, InMemoryEmployeesData>()
    .AddScoped<IProductData, SqlProductData>()
    .AddScoped<IOrderService, SqlOrderData>()
    .AddScoped<IVeiwAdminIndexData, AdminHomeIndexData>()

    .AddControllers().Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddIdentity<User, Role>(/*opt => { }*/)
        .AddEntityFrameworkStores<WebStoreDB>()
        .AddDefaultTokenProviders()
        .Services
        
    .Configure<IdentityOptions>(opt =>
    {
#if DEBUG
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 3;
        opt.Password.RequiredUniqueChars = 3;
#endif
        opt.User.RequireUniqueEmail = false;
        opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwqyxABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        opt.Lockout.AllowedForNewUsers = false;
        opt.Lockout.MaxFailedAccessAttempts = 10;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    });


WebApplication app = builder.Build();

using (IServiceScope? scope = app.Services.CreateScope())
{
    await scope.ServiceProvider
        .GetRequiredService<IDbInitializer>()
        .InitializeAsync(removeBefore: true);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();