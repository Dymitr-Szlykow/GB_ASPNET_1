namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public SectionInProductDTO Section { get; set; } = null!;
    public BrandInProductDTO? Brand { get; set; }

    public class SectionInProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public int? ParentId { get; set; }
    }

    public class BrandInProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
    }
}