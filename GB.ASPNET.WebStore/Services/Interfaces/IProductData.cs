using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;

namespace GB.ASPNET.WebStore.Services.Interfaces;

public interface IProductData
{
    IEnumerable<Section> GetSections();
    IEnumerable<Brand> GetBrands();
    IEnumerable<Product> GetProducts(ProductFilter? filter = null);
}
