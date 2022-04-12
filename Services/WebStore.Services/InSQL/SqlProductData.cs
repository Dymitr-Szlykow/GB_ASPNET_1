using Microsoft.EntityFrameworkCore;
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

    public Brand? GetBrandById(int id)
        => _dbContext.Brands
            .Include(el => el.Products)
            .FirstOrDefault(el => el.Id == id);

    public IEnumerable<Brand> GetBrands()
        => _dbContext.Brands;

    public Product? GetProductById(int id)
        => _dbContext.Products
            .Include(el => el.Section)
            .Include(el => el.Brand)
            .FirstOrDefault(el => el.Id == id);

    public IEnumerable<Product> GetProducts(ProductFilter? filter = null)
    {
        IQueryable<Product> products = _dbContext.Products
            .Include(el => el.Section)
            .Include(el => el.Brand);

        if (filter?.Ids?.Length > 0)
        {
            products = products.Where(el => filter.Ids.Contains(el.Id));
        }
        else
        {
            if (filter?.SectionId is { } sectionId)
                products = products.Where(el => el.SectionId == sectionId);

            if (filter?.BrandId is { } brandId)
                products = products.Where(el => el.BrandId == brandId);
        }

        return products;
    }

    public Section? GetSectionById(int id)
        => _dbContext.Sections
            .Include(el => el.Products)
            .FirstOrDefault(el => el.Id == id);

    public IEnumerable<Section> GetSections()
        => _dbContext.Sections;
}