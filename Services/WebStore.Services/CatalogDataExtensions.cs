using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Models;

public static class CatalogDataExtensions
{
    public static ProductViewModel? ToViewmodel(this Product? product)
        => product is null
            ? null
            : new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                BrandName = product.Brand?.Name,
                SectionName = product.Section.Name,
            };

    public static Product? ToEntityModel(this ProductViewModel? viewmodel)
        => viewmodel is null
            ? null
            : new Product
            {
                Id = viewmodel.Id,
                Name = viewmodel.Name,
                Price = viewmodel.Price,
                ImageUrl = viewmodel.ImageUrl
            };

    public static IEnumerable<ProductViewModel?> ToViewmodels(this IEnumerable<Product?> collection)
        => collection.Select(ToViewmodel);

    public static IEnumerable<Product?> ToEnityModels(this IEnumerable<ProductViewModel?> collection)
        => collection.Select(ToEntityModel);
}