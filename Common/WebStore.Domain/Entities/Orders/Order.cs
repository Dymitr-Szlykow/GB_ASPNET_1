using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GB.ASPNET.WebStore.Domain.Entities.Base;
using GB.ASPNET.WebStore.Domain.Entities.Identity;

namespace GB.ASPNET.WebStore.Domain.Entities.Orders;

public class Order : Entity
{
    [Required]
    public User User { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Phone { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

    [NotMapped]
    public decimal TotalOrderPrice => Items.Sum(item => item.TotalItemPrice);
}