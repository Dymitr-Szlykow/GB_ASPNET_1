using GB.ASPNET.WebStore.Data;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class InMemoryEmployeesData : IEmployeesData
{
    private readonly ILogger<InMemoryEmployeesData> _logger;
    private readonly ICollection<Employee> _employees;
    private int _LastFreeId;

    public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> logger)
    {
        _logger = logger;
        _employees = Data.TestData.Employees;
        _LastFreeId = _employees.Count == 0 ? 1 : _employees.Max(el => el.Id) + 1;
    }

    public int? Add(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));

        if (_employees.Contains(employee)) return employee.Id;
        else
        {
            employee.Id = _LastFreeId++;
            _employees.Add(employee);
            _logger.LogWarning("Добавлен сотрудник с id={0}.", employee.Id);
            return employee.Id;
        }
    }

    public bool Delete(int id)
    {
        var empToEdit = GetById(id);
        if (empToEdit == null)
        {
            _logger.LogWarning("Попытка удалить несуществующего сотрудника с id={0}.", id);
            return false;
        }
        else
        {
            _employees.Remove(empToEdit);
            _logger.LogWarning("Удален сотрудник с id={0}.", id);
            return true;
        }
    }

    public bool Edit(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));

        if (_employees.Contains(employee)) return true;

        var empToEdit = GetById(employee.Id);
        if (empToEdit == null)
        {
            _logger.LogWarning("Попытка редактирования отсутствующего сотрудника с id={0}.", employee.Id);
            return false;
        }
        else
        {
            _logger.LogWarning("Редактирование сотрудника с id={0}: {1}, возраст {2}.", employee.Id, employee.NameShort, employee.Age);
            empToEdit.NameFirst = employee.NameFirst;
            empToEdit.NameLast = employee.NameLast;
            empToEdit.NamePaternal = employee.NamePaternal;
            empToEdit.Age = employee.Age;
            _logger.LogWarning("Редактирован сотрудник с id={0}.", employee.Id);
            return true;
        }
    }

    public IEnumerable<Employee> GetAll()
    {
        return _employees;
    }

    public Employee? GetById(int id)
    {
        return _employees.FirstOrDefault(emp => emp.Id == id);
    }
}
