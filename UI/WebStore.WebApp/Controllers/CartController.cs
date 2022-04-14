using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.ViewModels;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.Controllers;

public class CartController : Controller
{
    private readonly ICart _CartService;
    private readonly ILogger<CartController> _logger;

    public CartController(ICart cartService, ILogger<CartController> logger)
    {
        _CartService = cartService;
        _logger = logger;
    }


    public IActionResult Add(int id, int? num)
    {
        _CartService.Add(id, null);
        return RedirectToAction("Index");
    }

    public IActionResult Clear()
    {
        _CartService.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Index()
        => View(new CartOrderVM { Cart = _CartService.GetViewmodel() });

    public IActionResult RemoveOne(int id)
    {
        _CartService.RemoveOne(id);
        return RedirectToAction("Index");
    }

    public IActionResult RemoveTitle(int id)
    {
        _CartService.RemoveTitle(id);
        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(OrderVM viewmodel, [FromServices] IOrderService orderService)
    {
        if (!ModelState.IsValid) return View(
            viewName: "Index",
            new CartOrderVM
            {
                Cart = _CartService.GetViewmodel(),
                Order = viewmodel
            });
        else
        {
            Order? order = await orderService.CreateOrderAsync(
                userName: User.Identity!.Name!,
                cart: _CartService.GetViewmodel(),
                order: viewmodel);
            _CartService.Clear();
            return RedirectToAction(nameof(ConfirmedOrder), new { order.Id });
        }
    }

    public IActionResult ConfirmedOrder(int id)
    {
        ViewBag.OrderId = id;
        return View();
    }
}