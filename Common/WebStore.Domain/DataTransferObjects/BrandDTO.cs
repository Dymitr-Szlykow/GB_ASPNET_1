namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class BrandDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public IEnumerable<ProductInBrandDTO> Products { get; set; } = null!;

    public class ProductInBrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int SectionId { get; set; }
    }
}