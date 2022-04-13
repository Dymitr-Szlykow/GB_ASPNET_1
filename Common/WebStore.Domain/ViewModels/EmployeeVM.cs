using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GB.ASPNET.WebStore.ViewModels;

public class EmployeeVM : IValidatableObject
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Имя является обязательным")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 255 символов")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата имени")]
    public string NameFirst { get; set; } = null!;

    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Фамилия является обязательной")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Длина фамилии должна быть от 2 до 255 символов")]
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата фамилии")]
    public string NameLast { get; set; } = null!;

    [Display(Name = "Отчество")]
    [StringLength(255, ErrorMessage = "Длина отчества должна быть до 255 символов")]
    [RegularExpression(@"(([А-ЯЁ][а-яё]+)|([A-Z][a-z]+))?", ErrorMessage = "Ошибка формата отчества")]
    public string? NamePaternal { get; set; }

    [Display(Name = "Ф.И.О.")]
    [StringLength(255, ErrorMessage = "Длина ФИО должна быть до 255 символов")]
    public string? NameShort { get; set; }

    [Display(Name = "Возраст")]
    [Range(18,80, ErrorMessage = "Возраст должен быть от 18 до 80")]
    public int Age { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NameLast.Length > 255)
            yield return new ValidationResult(
                "Длина фамилии превысила 255 символов",
                new[] { nameof(NameLast) });
        else
            yield return ValidationResult.Success!;
    }
}