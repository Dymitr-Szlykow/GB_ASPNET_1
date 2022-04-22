namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class OrderItemDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}