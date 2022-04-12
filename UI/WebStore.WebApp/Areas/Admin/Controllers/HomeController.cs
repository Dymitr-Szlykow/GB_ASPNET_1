using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.administrators)]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}