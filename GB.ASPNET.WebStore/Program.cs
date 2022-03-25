using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Infrastructure.Conventions;
using GB.ASPNET.WebStore.Infrastructure.Middleware;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.Services.Interfaces;


WebApplication
    .CreateBuilder(args)

    .SetMyServices()
    .Build()

    .SetUpMyDB()
    .SetMyMiddlewarePipeline()
    .MapMyRoutes()
    .Run();


public static class WebStoreBuildHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WebApplicationBuilder SetMyServices(this WebApplicationBuilder builder)
    {
        _ = builder.Services
            .AddDbContext<WebStoreDB>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
            )
            .AddTransient<IDbInitializer, DbInitializer>()
            .AddScoped<IEmployeesData, InMemoryEmployeesData>()
            //.AddScoped<IProductData, InMemoryProductData>();
            .AddScoped<IProductData, SqlProductData>();

        _ = builder.Services.AddControllersWithViews();
        //_ = builder.Services.AddControllersWithViews(opt =>
        //{
        //    opt.Conventions.Clear();
        //    opt.Conventions.Add(new DeletemeConvention());
        //});

        return builder;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WebApplication SetUpMyDB(this WebApplication app)
    {
        using (IServiceScope? scope = app.Services.CreateScope())
        {
            scope.ServiceProvider
                .GetRequiredService<IDbInitializer>()
                .InitializeAsync(removeBefore: true).RunSynchronously();
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
            .UseRouting();
            //.UseMiddleware<DeletemeMiddleware>();  // custom middleware

        return app;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static WebApplication MapMyRoutes(this WebApplication app)
    {
        _ = app.MapGet("/throw", handler: () => { throw new ApplicationException("Пример ошибки."); });

        _ = app.MapDefaultControllerRoute();
        //app.MapControllerRoute(
        //    name: "default",
        //    pattern: "{controller=Home}/{action=Index}/{id?}"
        //);

        return app;
    }
}