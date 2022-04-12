using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Controllers;

[Area("Admin")]
public class UserProfileController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
    {
        //User user = await _userManager.FindByNameAsync(User.Identity!.Name!);
        //if (user is null) ...
        IEnumerable<Order>? orders = await orderService.GetUserOrdersAsync(User.Identity!.Name!);
        return View(orders.Select(el => el.ToViewModel()));
    }
}