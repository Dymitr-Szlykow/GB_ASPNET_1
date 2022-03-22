using GB.ASPNET.WebStore.Infrastructure.Conventions;
using GB.ASPNET.WebStore.Infrastructure.Middleware;
using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.Services.Interfaces;


WebApplication
    .CreateBuilder(args)

    .SetMyServices()
    .Build()

    .SetMyMiddlewarePipeline()
    .MapMyRoutes()
    .Run();


public static class WebStoreBuildHelper
{
    public static WebApplicationBuilder SetMyServices(this WebApplicationBuilder builder)
    {
        _ = builder.Services
            .AddScoped<IEmployeesData, InMemoryEmployeesData>()
            .AddScoped<IProductData, InMemoryProducData>();

        _ = builder.Services.AddControllersWithViews();
        //_ = builder.Services.AddControllersWithViews(opt =>
        //{
        //    opt.Conventions.Clear();
        //    opt.Conventions.Add(new DeletemeConvention());
        //});

        return builder;
    }

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