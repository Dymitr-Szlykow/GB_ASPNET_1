using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Infrastructure.Conventions;
using GB.ASPNET.WebStore.Infrastructure.Middleware;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.WebAPI.Clients;

WebApplication
    .CreateBuilder(args)

    .SetMyServices()
    .Build()

    //.SetUpMyDB().Result //.GetAwaiter().GetResult()
    .SetMyMiddlewarePipeline()
    .MapMyRoutes()
    .Run();


public static class WebStoreBuildHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WebApplicationBuilder SetMyServices(this WebApplicationBuilder builder)
    {
        _ = builder.Services
            .AddScoped<ICart, InCookiesCart>()
            .AddScoped<IVeiwAdminIndexData, AdminHomeIndexData>()

            .AddHttpClient("WebStoreApi", http => http.BaseAddress = new Uri(builder.Configuration["WebAPI"]))
                .AddTypedClient<IValuesAPI, ValuesClient>()
                .AddTypedClient<IEmployeesData, EmployeesClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrderService, OrdersClient>()
                .Services

            .AddAutoMapper(typeof(Program)) //.AddAutoMapper(Assembly.GetEntryAssembly());

            .AddIdentity<User, Role>(/*opt => { }*/)
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders()
                .Services
            .AddHttpClient("WebStoreApiIdentity", http => http.BaseAddress = new Uri(builder.Configuration["WebAPI"]))
                .AddTypedClient<IUserStore<User>, UsersClient>()
                .AddTypedClient<IUserRoleStore<User>, UsersClient>()
                .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
                .AddTypedClient<IUserEmailStore<User>, UsersClient>()
                .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
                .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
                .AddTypedClient<IUserLoginStore<User>, UsersClient>()
                .AddTypedClient<IUserClaimStore<User>, UsersClient>()
                .AddTypedClient<IRoleStore<Role>, RolesClient>()
                .Services

            .AddControllersWithViews(opt =>
            {
                //opt.Conventions.Add(new DeletemeConvention());
                opt.Conventions.Add(new SetAreaControllersRoute());
            });

        _ = builder.Services

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
            })

            .ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "GBWebStore";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";
                opt.SlidingExpiration = true;
            });

        return builder;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<WebApplication> SetUpMyDB(this WebApplication app)
    {
        using (IServiceScope? scope = app.Services.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IDbInitializer>()
                .InitializeAsync(removeBefore: true);
        }
        return app;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WebApplication SetMyMiddlewarePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }

        _ = app
            .UseStaticFiles()
            .UseRouting()
            //.UseMiddleware<DeletemeMiddleware>()  // custom middleware
            .UseAuthentication()
            .UseAuthorization();

        return app;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WebApplication MapMyRoutes(this WebApplication app)
    {
        _ = app.MapGet("/throw", handler: () => { throw new ApplicationException("Пример ошибки."); });

        _ = app.UseEndpoints(
            endpoints =>
            {
                _ = endpoints.MapControllerRoute(
                    name: "ActionRoute",
                    pattern: "{controller}.{action}({a}, {b})"
                );
                _ = endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                _ = endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );//.MapDefaultControllerRoute();
            });

        return app;
    }
}