using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain;
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
                Products =
                    products
                    .OrderBy(el => el.Order)
                    .Select(
                        el => new ProductViewModel()
                        {
                            Id = el.Id,
                            Name = el.Name,
                            Price = el.Price,
                            ImageUrl = el.ImageUrl,
                        }),
            });
    }
}