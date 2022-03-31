using GB.ASPNET.WebStore.ViewModels;
using GB.ASPNET.WebStore.Domain.Entities;

namespace GB.ASPNET.WebStore.Models;

public static class EmployeeDataExtensions
{
    public static EmployeeVM ToViewmodel(this Employee emp)
    {
        return new EmployeeVM
        {
            Id = emp.Id,
            NameFirst = emp.NameFirst,
            NameLast = emp.NameLast,
            NamePaternal = emp.NamePaternal,
            NameShort = emp.NameShort,
            Age = emp.Age,
        };
    }

    public static Employee ToEntityModel(this EmployeeVM viewmodel)
    {
        return new Employee
        {
            Id = viewmodel.Id,
            NameFirst = viewmodel.NameFirst,
            NameLast = viewmodel.NameLast,
            NamePaternal = viewmodel.NamePaternal,
            Age = viewmodel.Age
        };
    }
}
