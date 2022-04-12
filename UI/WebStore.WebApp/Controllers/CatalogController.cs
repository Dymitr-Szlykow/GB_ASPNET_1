using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.Services.Interfaces;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Controllers;

public class CatalogController : Controller
{
    private readonly IProductData _data;

    public CatalogController(IProductData data) => _data = data;

    public IActionResult Index(int? sectionId, int? brandId)
    {
        var filter = new ProductFilter()
        {
            BrandId = brandId,
            SectionId = sectionId,
        };
        var products = _data.GetProducts();

        return View(
            new CatalogViewModel()
            {
                BrandId = filter.BrandId,
                SectionId = filter.SectionId,
                Products = products
                    .OrderBy(el => el.Order)
                    .ToViewmodels()!
            });
    }

    public IActionResult Details(int id)
    {
        var product = _data.GetProductById(id);
        if (product is null) return NotFound();
        else
        {
            ViewBag.Products = _data
                .GetProducts()
                .OrderBy(el => el.Order)
                .Take(6)
                .ToViewmodels();
            return View(product.ToViewmodel());
        }
    }
}