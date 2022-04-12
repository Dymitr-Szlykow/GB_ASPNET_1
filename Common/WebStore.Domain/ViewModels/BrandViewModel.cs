using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASPNET.WebStore.ViewModels;

public class BrandViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Наименование бренда")]
    [Required(ErrorMessage = "Наименование бренда является обязательным")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина наименования бренда должна быть от 2 до 255 символов")]
    public string Name { get; set; } = null!;
}