using System.ComponentModel.DataAnnotations;

namespace GB.ASPNET.WebStore.ViewModels;

public class OrderVM
{
    [Required, MaxLength(200)]
    public string Address { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Phone { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }
}