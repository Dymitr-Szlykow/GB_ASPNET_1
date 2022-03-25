using System.Text;
using GB.ASPNET.WebStore.Domain.Entities.Base;

namespace GB.ASPNET.WebStore.Domain.Entities;

public class Employee : Entity
{
    private int age;

    public string NameFirst { get; set; } = null!;
    public string NameLast { get; set; } = null!;
    public string? NamePaternal { get; set; }

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

    public int Age
    {
        get => age;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "Значение возраста не может быть меньше нуля");
            age = value;
        }
    }
}
