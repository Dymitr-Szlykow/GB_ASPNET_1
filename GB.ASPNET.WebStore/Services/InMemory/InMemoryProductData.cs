using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class InMemoryProductData : IProductData
{
    public IEnumerable<Brand> GetBrands() => TestData.Brands;

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        IEnumerable<Product> query = TestData.Products;

        //if (filter != null && filter.SectionId != null)
        //    query = query.Where(el => el.SectionId == filter.SectionId);

        if (filter?.SectionId is { } sectionId)
            query = query.Where(el => el.SectionId == sectionId);

        if (filter?.BrandId is { } brandId)
            query = query.Where(el => el.BrandId == brandId);

        return query;
    }

    public IEnumerable<Section> GetSections() => TestData.Sections;
}