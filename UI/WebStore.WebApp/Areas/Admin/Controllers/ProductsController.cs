using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Role.administrators)]
public class ProductsController : Controller
{
    private readonly IProductData _productData;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductData productData, ILogger<ProductsController> logger)
    {
        _productData = productData;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(_productData.GetProducts());
    }
}