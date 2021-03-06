using System.ComponentModel.DataAnnotations;

namespace GB.ASPNET.WebStore.ViewModels.Identity;

public class RegisterUserVM
{
    [Required]
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; } = null!;

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; } = null!;
}