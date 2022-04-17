using GB.ASPNET.WebStore.Domain.DataTransferObjects;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Models;

public static class UserDataExtensions
{
    public static UserOrderViewModel? ToViewModel(this Order? model)
    {
        return model is null
            ? null
            : new UserOrderViewModel
            {
                Id = model.Id,
                Phone = model.Phone,
                Address = model.Address,
                Description = model.Description,
                TotalPrice = model.TotalOrderPrice,
                Date = model.Date
            };
    }

    public static Order? ToEntityModel(this UserOrderViewModel? viewmodel, User user)
    {
        return viewmodel is null
            ? null
            : new Order
            {
                Id = viewmodel.Id,
                User = user,
                Phone = viewmodel.Phone,
                Address = viewmodel.Address,
                Description = viewmodel.Description,
                Date = viewmodel.Date
            };
    }

    public static IEnumerable<OrderDTO>? ToDtoModels(this IEnumerable<Order>? entities) => entities?.Select(ToDtoModel)!;
    public static OrderDTO? ToDtoModel(this Order? entity)
    {
        return entity is null
            ? null
            : new OrderDTO
            {
                Id = entity.Id,
                UserName = entity.User.UserName,
                Phone = entity.Phone,
                Address = entity.Address,
                Description = entity.Description,
                Date = entity.Date,
                Items = entity.Items.Select(item => new OrderItemDTO
                {
                    Id = item.Id,
                    ProductId = item.Product.Id,
                    Price = item.Price,
                    Quantity = item.Quantity
                })
            };
    }

    public static IEnumerable<Order>? ToEntityModels(this IEnumerable<OrderDTO>? dto, User? user) => dto?.Select(one => one.ToEntityModel(user))!;
    public static Order? ToEntityModel(this OrderDTO? dto, User? user)
    {
        if (dto is null) return null;

        var result = new Order
        {
            Id = dto.Id,
            Phone = dto.Phone,
            Address = dto.Address,
            Description = dto.Description,
            Items = new HashSet<OrderItem>(dto.Items.Select(item => new OrderItem
            {
                Id = item.Id,
                Product = new Product { Id = item.ProductId },
                Price = item.Price,
                Quantity = item.Quantity
            }))
        };
        if (user is not null) result.User = user;
        foreach (OrderItem? item in result.Items) item.Order = result;
        return result;
    }

    public static IEnumerable<OrderItemDTO> ToDtoModels(this CartVM viewmodel)
    {
        return viewmodel.Items.Select(item => new OrderItemDTO
        {
            ProductId = item.Product.Id,
            Price = item.Product.Price,
            Quantity = item.Quantity
        });
    }

    public static CartVM ToCartVM(this IEnumerable<OrderItemDTO> collection)
    {
        return new CartVM
        {
            Items = collection.Select(item =>
            (
                new ProductViewModel { Id = item.ProductId },
                item.Quantity)
            )
        };
    }
}