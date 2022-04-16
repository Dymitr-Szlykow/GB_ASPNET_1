namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class SectionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public int? ParentId { get; set; }
}