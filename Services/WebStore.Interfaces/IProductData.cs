using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;

namespace GB.ASPNET.WebStore.Interfaces;

public interface IProductData
{
    IEnumerable<Section> GetSections();
    Section? GetSectionById(int id);

    IEnumerable<Brand> GetBrands();
    Brand? GetBrandById(int id);

    IEnumerable<Product> GetProducts(ProductFilter? filter = null);
    Product? GetProductById(int id);
}