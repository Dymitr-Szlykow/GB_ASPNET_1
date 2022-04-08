using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Domain.Entities.Base;

namespace GB.ASPNET.WebStore.Domain.Entities.Orders;

public class OrderItem : Entity
{
    [Required]
    public Product Product { get; set; }

    [Column(TypeName = "decimal(18,2")]
    public decimal Price { get; set; }

    [NotMapped]
    public decimal TotalItemPrice => Price * Quantity;

    public int Quantity { get; set; }

    [Required]
    public Order Order { get; set; } = null!;
}