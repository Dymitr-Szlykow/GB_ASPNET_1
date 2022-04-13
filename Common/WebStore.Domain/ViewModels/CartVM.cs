using GB.ASPNET.WebStore.Domain.Entities;

namespace GB.ASPNET.WebStore.ViewModels;

public class CartVM
{
    public IEnumerable<(ProductViewModel Product, int Quantity)> Items { get; set; } = null!;

    public int ItemsCount => Items.Sum(item => item.Quantity);
    public decimal TotalPrice => Items.Sum(item => item.Product.Price * item.Quantity);
}