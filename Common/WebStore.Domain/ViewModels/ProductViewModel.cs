using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASPNET.WebStore.ViewModels;

public class ProductViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Наименование товара")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина наименования товара должна быть от 2 до 255 символов")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата наименования товара")]
    public string Name { get; set; } = null!;

    [Display(Name = "Цена")]
    public decimal Price { get; set; }

    [HiddenInput(DisplayValue = false)]
    [Required(ErrorMessage = "Путь к изображению товара является обязательным.")]
    public string ImageUrl { get; set; } = null!;

    public string? BrandName { get; set; }
    public string SectionName { get; set; } = null!;
}
