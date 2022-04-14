using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.Services;

[Obsolete("Устарел. Использется БД: SqlProductData", error: true)]
public class InMemoryProductData : IProductData
{
    public Brand? GetBrandById(int id) => TestData.Brands.FirstOrDefault(b => b.Id == id);

    public IEnumerable<Brand> GetBrands() => TestData.Brands;

    public Product? GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id);

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

    public Section? GetSectionById(int id) => TestData.Sections.FirstOrDefault(el => el.Id == id);

    public IEnumerable<Section> GetSections() => TestData.Sections;
}