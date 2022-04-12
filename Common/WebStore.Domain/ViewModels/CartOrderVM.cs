namespace GB.ASPNET.WebStore.ViewModels;

public class CartOrderVM
{
    public CartVM Cart { get; set; } = null!;
    public OrderVM Order { get; set; } = new();
}