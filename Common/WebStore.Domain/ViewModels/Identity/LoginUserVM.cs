using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASPNET.WebStore.ViewModels.Identity;

public class LoginUserVM
{
    [Required]
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; } = null!;

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; } = false;

    [HiddenInput(DisplayValue = false)]
    public string? ReturnUrl { get; set; } = null!;
}