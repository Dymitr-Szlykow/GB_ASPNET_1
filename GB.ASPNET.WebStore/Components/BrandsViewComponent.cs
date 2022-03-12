using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Services.Interfaces;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Components;

public class BrandsViewComponent : ViewComponent
{
    private readonly IProductData _data;

    public BrandsViewComponent(IProductData data) => _data = data;

    public IViewComponentResult Invoke() => View(GetBrands());

    public IEnumerable<BrandViewModel> GetBrands()
        => _data
            .GetBrands()
            .OrderBy(b => b.Order)
            .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
            });

    //public Task<IViewComponentResult> InvokeAsync()
    //{
    //    //
    //}
}
