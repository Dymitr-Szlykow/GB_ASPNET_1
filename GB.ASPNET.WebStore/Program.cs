var builder = WebApplication.CreateBuilder(args);
_ = builder.Services.AddControllersWithViews();

// регистрация сервисов

var app = builder.Build();
_ = app.UseRouting();

_ = app.MapGet("/greetings", () => app.Configuration["ServerGreetings"]);
//_ = app.MapPost("/home/employeeupdate/{id:number}", (id) => )

_ = app.MapDefaultControllerRoute();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"
//    );

app.Run();
