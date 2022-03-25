using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class SqlProductData : IProductData
{
    private readonly WebStoreDB _dbContext;
    private readonly ILogger<SqlProductData> _logger;

    public SqlProductData(WebStoreDB dbContext, ILogger<SqlProductData> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IEnumerable<Brand> GetBrands()
    {
        return _dbContext.Brands;
    }

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        IQueryable<Product> products = _dbContext.Products;

        if (filter?.SectionId is { } sectionId)
            products = products.Where(p => p.SectionId == sectionId);

        if (filter?.BrandId is { } brandId)
            products = products.Where(p => p.BrandId == brandId);

        return products;
    }

    public IEnumerable<Section> GetSections()
    {
        return _dbContext.Sections;
    }
}