using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.ViewModels;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.Services.Interfaces;

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
        _CartService.Add(id);
        return RedirectToAction("Index");
    }

    public IActionResult Clear()
    {
        _CartService.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Index()
        => View(_CartService.GetViewmodel());

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
}