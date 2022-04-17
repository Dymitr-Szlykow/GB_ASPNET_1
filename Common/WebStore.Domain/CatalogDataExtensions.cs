using GB.ASPNET.WebStore.Domain.DataTransferObjects;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Models;

public static class CatalogDataExtensions
{
    #region PRODUCTS

    public static IEnumerable<ProductViewModel>? ToViewmodels(this IEnumerable<Product>? entities) => entities?.Select(ToViewmodel)!;
    public static ProductViewModel? ToViewmodel(this Product? entity)
    {
        return entity is null ? null : new ProductViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            ImageUrl = entity.ImageUrl,
            BrandName = entity.Brand?.Name,
            SectionName = entity.Section.Name
        };
    }

    public static IEnumerable<ProductDTO>? ToDtoModels(this IEnumerable<Product>? entities) => entities?.Select(ToDtoModel)!;
    public static ProductDTO? ToDtoModel(this Product? entity)
    {
        return entity is null ? null : new ProductDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            Price = entity.Price,
            ImageUrl = entity.ImageUrl,
            Section = new ProductDTO.SectionInProductDTO
            {
                Id = entity.Section.Id,
                Name = entity.Section.Name,
                Order = entity.Section.Order,
                ParentId = entity.Section.ParentId
            },
            Brand = entity.Brand is null ? null : new ProductDTO.BrandInProductDTO
            {
                Id = entity.Brand.Id,
                Name = entity.Brand.Name,
                Order = entity.Brand.Order
            }
        };
    }

    public static IEnumerable<Product>? ToEntityModels(this IEnumerable<ProductViewModel>? viewmodels) => viewmodels?.Select(ToEntityModel)!;
    public static Product? ToEntityModel(this ProductViewModel? viewmodel)
    {
        return viewmodel is null ? null : new Product
        {
            Id = viewmodel.Id,
            Name = viewmodel.Name,
            Price = viewmodel.Price,
            ImageUrl = viewmodel.ImageUrl
        };
    }

    public static IEnumerable<Product>? ToEntityModels(this IEnumerable<ProductDTO>? dtoModels) => dtoModels?.Select(ToEntityModel)!;
    public static Product? ToEntityModel(this ProductDTO? dto)
    {
        return dto is null ? null : new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Order = dto.Order,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            SectionId = dto.Section.Id,
            Section = new Section
            {
                Id = dto.Section.Id,
                Name = dto.Section.Name,
                Order = dto.Section.Order,
                ParentId = dto.Section.ParentId
                // список товаров секции остается пустым HashSet<Product>
            },
            BrandId = dto.Brand?.Id,
            Brand = dto.Brand is null ? null : new Brand
            {
                Id = dto.Brand.Id,
                Name = dto.Brand.Name,
                Order = dto.Brand.Order
                // список товаров бренда остается пустым HashSet<Product>
            }
        };
    }

    #endregion

    #region BRANDS

    public static IEnumerable<BrandDTO>? ToDtoModels(this IEnumerable<Brand>? dtoModels) => dtoModels?.Select(ToDtoModel)!;
    public static BrandDTO? ToDtoModel(this Brand? entity)
    {
        return entity is null ? null : new BrandDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            Products = entity.Products.Select(product => new BrandDTO.ProductInBrandDTO
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SectionId = product.Section.Id
            })
        };
        }

    public static IEnumerable<Brand>? ToEntityModels(this IEnumerable<BrandDTO>? dtoModels) => dtoModels?.Select(ToEntityModel)!;
    public static Brand? ToEntityModel(this BrandDTO? dto)
    {
        if (dto is null) return null;

        var result = new Brand
        {
            Id = dto.Id,
            Name = dto.Name,
            Order = dto.Order,
            Products = new HashSet<Product>(dto.Products.Select(product => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SectionId = product.SectionId,
                // Section остаются null
                BrandId = dto.Id
            }))
        };
        foreach (Product item in result.Products) item.Brand = result;
        return result;
    }

    #endregion

    #region SECTIONS

    public static IEnumerable<SectionDTO>? ToDtoModels(this IEnumerable<Section>? dtoModels) => dtoModels?.Select(ToDtoModel)!;
    public static SectionDTO? ToDtoModel(this Section? entity)
    {
        return entity is null ? null : new SectionDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Order = entity.Order,
            ParentId = entity.ParentId,
            Products = entity.Products.Select(product => new SectionDTO.ProductInSectionDTO
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name
            })
        };
    }

    public static IEnumerable<Section>? ToEntityModels(this IEnumerable<SectionDTO>? dtoModels) => dtoModels?.Select(ToEntityModel)!;
    public static Section? ToEntityModel(this SectionDTO? dto)
    {
        if (dto is null) return null;

        var result = new Section
        {
            Id = dto.Id,
            Name = dto.Name,
            Order = dto.Order,
            ParentId = dto.ParentId,
            Products = new HashSet<Product>(dto.Products.Select(product => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SectionId = dto.Id,
                BrandId = product.BrandId,
                Brand = product.BrandName is null ? null : new Brand
                {
                    Id = product.BrandId!.Value,
                    Name = product.BrandName
                }
            }))
        };
        foreach (var item in result.Products) item.Section = result;
        return result;
    }

    #endregion

    /*
     * много труда, чтобы восстановить вторичные и третичные связи БД
     * и DTO получаются крупноваты для передачи
     * контроллер получает урезанные модели от одной из реализаций IProductData
     * может в IProductData добавить delegate?, получающий списки товаров, брендов, секций и сшивающий недостающие ссылки
     */
}