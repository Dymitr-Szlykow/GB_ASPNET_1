var builder = WebApplication.CreateBuilder(args);

// регистрация сервисов
_ = builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

_ = app.UseStaticFiles();
_ = app.UseRouting();

//_ = app.MapGet("/greetings", () => app.Configuration["ServerGreetings"]);
_ = app.MapGet("/throw", handler: () => { throw new ApplicationException("Пример ошибки."); });
//_ = app.MapPost("/home/employeeupdate/{id:number}", (id) => );

_ = app.MapDefaultControllerRoute();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}"
//    );

app.Run();