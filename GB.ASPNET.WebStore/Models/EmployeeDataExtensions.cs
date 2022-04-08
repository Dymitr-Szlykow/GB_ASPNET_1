using GB.ASPNET.WebStore.ViewModels;
using GB.ASPNET.WebStore.Domain.Entities;

namespace GB.ASPNET.WebStore.Models;

public static class EmployeeDataExtensions
{
    public static EmployeeVM? ToViewmodel(this Employee? emp)
        => emp is null
            ? null
            : new EmployeeVM
            {
                Id = emp.Id,
                NameFirst = emp.NameFirst,
                NameLast = emp.NameLast,
                NamePaternal = emp.NamePaternal,
                NameShort = emp.NameShort,
                Age = emp.Age
            };

    public static Employee? ToEntityModel(this EmployeeVM? viewmodel)
        => viewmodel is null
            ? null
            : new Employee
            {
                Id = viewmodel.Id,
                NameFirst = viewmodel.NameFirst,
                NameLast = viewmodel.NameLast,
                NamePaternal = viewmodel.NamePaternal,
                Age = viewmodel.Age
            };

    public static IEnumerable<EmployeeVM?> ToViewmodels(this IEnumerable<Employee?> collection)
        => collection.Select(ToViewmodel);

    public static IEnumerable<Employee?> ToEntitymodels(this IEnumerable<EmployeeVM?> collection)
        => collection.Select(ToEntityModel);
}
