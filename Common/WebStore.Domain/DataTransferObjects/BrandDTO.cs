namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class BrandDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
}