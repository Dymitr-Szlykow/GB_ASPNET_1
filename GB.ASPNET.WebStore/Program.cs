using GB.ASPNET.WebStore.Infrastructure.Conventions;
using GB.ASPNET.WebStore.Infrastructure.Middleware;
using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// р е г и с т р а ц и я  с е р в и с о в
_ = builder.Services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
_ = builder.Services.AddScoped<IProductData, InMemoryProducData>();
_ = builder.Services.AddControllersWithViews();
//_ = builder.Services.AddControllersWithViews(opt =>
//{
//    opt.Conventions.Clear();
//    opt.Conventions.Add(new DeletemeConvention());
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    _ = app.UseDeveloperExceptionPage();
_ = app.UseStaticFiles();
_ = app.UseRouting();
// _ = app.UseMiddleware<DeletemeMiddleware>();  // custom middleware

_ = app.MapGet("/throw", handler: () => { throw new ApplicationException("Пример ошибки."); });
//_ = app.MapPost("/home/employeeupdate/{id:number}", (id) => );

_ = app.MapDefaultControllerRoute();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"
//);

app.Run();