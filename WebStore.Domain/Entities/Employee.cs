using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Domain.Entities.Base;

namespace GB.ASPNET.WebStore.Domain.Entities;

[Index(nameof(NameLast), nameof(NameFirst), IsUnique = false)]
public class Employee : Entity
{
    private int age;

    [Required]
    public string NameFirst { get; set; } = null!;

    [Required]
    public string NameLast { get; set; } = null!;
    public string? NamePaternal { get; set; }

    [NotMapped]
    public string NameShort
    {
        get
        {
            var result = new StringBuilder(NameLast);

            if (NameFirst is { Length: > 0 })
                _ = result.Append(' ').Append(NameFirst[0]).Append('.');
            if (NamePaternal is { Length: > 0 })
                _ = result.Append(' ').Append(NamePaternal[0]).Append('.');

            return result.ToString();
        }
    }

    [Required]
    public int Age
    {
        get => age;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), value, "Значение возраста должно быть положительным.");
            age = value;
        }
    }
}
