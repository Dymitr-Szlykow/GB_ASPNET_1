using System.Net;
using System.Net.Http.Json;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.DataTransferObjects;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public class ProductsClient : BaseClient, IProductData
{
    public ProductsClient(HttpClient httpClient) : base(httpClient, "api/catalog") { }


    public Brand? GetBrandById(int id)
    {
        var brand = Get<BrandDTO>($"{ControllerRoute}/brands/{id}");
        return brand.ToEntityModel();
    }

    public IEnumerable<Brand> GetBrands()
    {
        var list = Get<IEnumerable<BrandDTO>>($"{ControllerRoute}/brands");
        return list.ToEntityModels() ?? Enumerable.Empty<Brand>();
    }

    public Product? GetProductById(int id)
    {
        var product = Get<ProductDTO>($"{ControllerRoute}/products/{id}");
        return product.ToEntityModel();
    }

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        HttpResponseMessage? response = Post<ProductFilter>($"{ControllerRoute}/products", filter ?? new ProductFilter());
        var list = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
        return list.ToEntityModels() ?? Enumerable.Empty<Product>();
    }

    public Section? GetSectionById(int id)
    {
        var section = Get<SectionDTO>($"{ControllerRoute}/sections/{id}");
        return section.ToEntityModel();
    }

    public IEnumerable<Section> GetSections()
    {
        var list = Get<IEnumerable<SectionDTO>>($"{ControllerRoute}/sections");
        return list.ToEntityModels() ?? Enumerable.Empty<Section>();
    }
}