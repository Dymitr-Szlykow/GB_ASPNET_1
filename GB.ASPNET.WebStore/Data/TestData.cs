using GB.ASPNET.WebStore.Models;

namespace GB.ASPNET.WebStore.Data;

public class TestData
{
    public static ICollection<Employee> Employees { get; } = new List<Employee>()
        {
            new Employee { Id = 1, NameLast = "Иванов", NameFirst = "Иван", NamePaternal = "Иванович", Age = 23 },
            new Employee { Id = 2, NameLast = "Петров", NameFirst = "Петр", NamePaternal = "Петрович", Age = 27 },
            new Employee { Id = 3, NameLast = "Сидоров", NameFirst = "Сидор", NamePaternal = "Сидорович", Age = 18 }
        };
}
