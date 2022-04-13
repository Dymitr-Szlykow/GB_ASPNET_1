using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Domain.Entities.Orders;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Models;

public static class UserDataExtensions
{
    public static UserOrderViewModel? ToViewModel(this Order? model)
        => model is null
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

    public static Order? ToEntityModel(this UserOrderViewModel? viewmodel, User user)
        => viewmodel is null
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