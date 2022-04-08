using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken token = default);

    Task<Order> GetOrderByIdAsync(int id, CancellationToken token = default);

    Task<Order> CreateOrderAsync(string userName, CartVM cart, OrderVM order, CancellationToken token = default);
}