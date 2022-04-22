using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.DataTransferObjects;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public class OrdersClient : BaseClient, IOrderService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<OrdersClient> _logger;

    public OrdersClient(HttpClient httpClient, UserManager<User> userManager, ILogger<OrdersClient> logger) : base(httpClient, "api/orders")
    {
        _userManager = userManager;
        _logger = logger;
    }


    public async Task<Order> CreateOrderAsync(string userName, CartVM cart, OrderVM order, CancellationToken token = default)
    {
        var bundle = new CreateOrderDTO
        {
            Order = order,
            Items = cart.ToDtoModels()
        };

        HttpResponseMessage? response = await PostAsync($"{ControllerRoute}/{userName}", bundle).ConfigureAwait(false);
        OrderDTO? result = await response
            .EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<OrderDTO>(cancellationToken: token)
            .ConfigureAwait(continueOnCapturedContext: false);

        User? user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            _logger.LogError("Не удалось найти пользователя по имени \"{UserName}\" (из DTO: {result}).", userName, result);
            throw new ArgumentException($"Не удалось найти пользователя по имени {userName}.");
        }
        else
        {
            if (result!.UserName != userName)
                _logger.LogWarning("Несоответствие имени пользователя \"{userName}\" (аргумент) и полученного заказа: {result}.", userName, result);
            return result.ToEntityModel(user)!;
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken token = default)
    {
        var order = await GetAsync<OrderDTO>($"{ControllerRoute}/{id}").ConfigureAwait(false);
        if (order is not null)
        {
            User? user = await _userManager.FindByNameAsync(order.UserName);
            if (user is null)
            {
                _logger.LogError("Не удалось найти пользователя по имени \"{UserName}\" (из DTO: {order}).", order.UserName, order);
                throw new InvalidDataException($"Не удалось найти пользователя по имени {order.UserName}.");
            }
            else return order.ToEntityModel(user);
        }
        else return null;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken token = default)
    {
        User? user = await _userManager.FindByNameAsync(userName);
        if (user is null)
        {
            _logger.LogError("Не удалось найти пользователя по имени \"{userName}\" (аргумент).", userName);
            throw new ArgumentException($"Не удалось найти пользователя по имени {userName}.", nameof(userName));
        }
        else
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{ControllerRoute}/user/{userName}").ConfigureAwait(false);
            if (orders!.Any(one => one.UserName != userName))
                _logger.LogWarning("Несоответствие имени пользователя \"{userName}\" (аргумент) и полученного списока заказов: {orders}.", userName, orders);
            return orders.ToEntityModels(user)!;
        }
    }
}