namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class SectionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public IEnumerable<ProductInSectionDTO> Products { get; set; } = null!;

    public class ProductInSectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
    }
}