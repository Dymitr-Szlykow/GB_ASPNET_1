using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASPNET.WebStore.ViewModels;

public class SectionViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Наименование отдела")]
    [Required(ErrorMessage = "Наименование отдела является обязательным")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина наименования отдела должна быть от 2 до 255 символов")]
    public string Name { get; set; } = null!;

    [HiddenInput(DisplayValue = false)]
    public int Order { get; set; }

    public List<SectionViewModel> Children { get; set; } = new List<SectionViewModel>();
}
