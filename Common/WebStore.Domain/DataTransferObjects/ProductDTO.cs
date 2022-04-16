namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public SectionDTO Section { get; set; } = null!;
    public BrandDTO Brand { get; set; } = null!;
}