using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Domain.DataTransferObjects;

public class OrderDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset Date { get; set; }
    public IEnumerable<OrderItemDTO> Items { get; set; } = null!;
}

public class CreateOrderDTO
{
    public OrderVM Order { get; set; } = null!;

    public IEnumerable<OrderItemDTO> Items { get; set; } = null!;
}