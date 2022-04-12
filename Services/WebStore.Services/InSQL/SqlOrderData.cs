using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.Services.Interfaces;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Services;

public class SqlOrderData : IOrderService
{
    private readonly WebStoreDB _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<SqlOrderData> _logger;

    public SqlOrderData(WebStoreDB dbContext, UserManager<User> userManager, ILogger<SqlOrderData> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _logger = logger;
    }


    public async Task<Order> CreateOrderAsync(string userName, CartVM cart, OrderVM orderModel, CancellationToken token = default)
    {
        var user = await _userManager.FindByNameAsync(userName).ConfigureAwait(continueOnCapturedContext: false);
        if (user is null)
        {
            _logger.LogError("Создание заказа: пользователь с именем {userName} в системе не найден.", userName);
            throw new InvalidOperationException($"Пользователь с именем {userName} в системе не найден.");
        }
        else
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(token).ConfigureAwait(false);

            var order = new Order
            {
                User = user,
                Address = orderModel.Address,
                Phone = orderModel.Phone,
                Description = orderModel.Description,
            };

            int[]? ids = cart.Items.Select(el => el.Product.Id).ToArray();
            var products = await _dbContext.Products
                .Where(el => ids.Contains(el.Id))
                .ToArrayAsync(token)
                .ConfigureAwait(continueOnCapturedContext: false);
            order.Items = cart.Items.Join(
                products,
                item => item.Product.Id,
                product => product.Id,
                (item,product) => new OrderItem
                {
                    Order = order,
                    Product = product,
                    Price = product.Price, // применять скидки здесь
                    Quantity = item.Quantity,
                })
                .ToArray();

            await _dbContext.Orders.AddAsync(order, token).ConfigureAwait(continueOnCapturedContext: false);
            await _dbContext.SaveChangesAsync(token).ConfigureAwait(continueOnCapturedContext: false);
            await transaction.CommitAsync(token).ConfigureAwait(false);
            return order;
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken token = default)
    {
        var order = await _dbContext.Orders
            .Include(el => el.User)
            .Include(el => el.Items)
            .ThenInclude(el => el.Product)
            .FirstOrDefaultAsync(order => order.Id == id)
            .ConfigureAwait(continueOnCapturedContext: false);
        return order;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userName, CancellationToken token = default)
    {
        var orders = await _dbContext.Orders
            .Include(el => el.User)
            .Include(el => el.Items)
            .ThenInclude(el => el.Product)
            .Where(order => order.User.UserName == userName)
            .ToArrayAsync(token)
            .ConfigureAwait(continueOnCapturedContext: false);
        return orders;
    }
}