using GB.ASPNET.WebStore.Domain.Entities.Base;
using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities;

public class Product : NamedEntity, IOrderedEntity
{
    public int Order { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;

    public int SectionId { get; set; }
    public int? BrandId { get; set; }
}