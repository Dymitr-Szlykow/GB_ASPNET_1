using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Clients;

public class EmployeesClient : BaseClient, IEmployeesData
{
    private readonly ILogger<EmployeesClient> _logger;

    public EmployeesClient(HttpClient httpClient, ILogger<EmployeesClient> logger)
        : base(httpClient, "api/employees")
    {
        _logger = logger;
    }


    public int? Add(Employee employee)
    {
        HttpResponseMessage? response = Post<Employee>(ControllerRoute, employee);
        Employee? newEntry = response.Content.ReadFromJsonAsync<Employee>().Result;
        if (newEntry is null)
        {
            // EnsureSuccessStatusCode() проверяется в BaseClient.Post() перед возвращением. Это достижимый код?
            _logger.LogError("Ошибка при создании записи сотрудника: {employee}", employee);
            return -1;
        }
        else
        {
            _logger.LogInformation("Создана запись сотрудника: {employee}", employee);
            return employee.Id = newEntry.Id;
        }
    }

    public bool Delete(int id)
    {
        HttpResponseMessage? response = Delete($"{ControllerRoute}/{id}");
        bool success = response.IsSuccessStatusCode;
        if (success) _logger.LogInformation("Удалена запись сотрудника с id={id}", id);
            // информации недостаточно, вероятно лучше логи вести из контроллера WebApp с занесением в них целых моделей
        return success;
    }

    public bool Edit(Employee employee)
    {
        HttpResponseMessage? response = Put<Employee>(ControllerRoute, employee);
        // EnsureSuccessStatusCode() проверяется в BaseClient.Put() перед возвращением
        return response.Content.ReadFromJsonAsync<bool>().Result;
    }

    public IEnumerable<Employee> GetAll()
    {
        IEnumerable<Employee>? employees = Get<IEnumerable<Employee>>(ControllerRoute);
        return employees ?? Enumerable.Empty<Employee>();
    }

    public Employee? GetById(int id)
    {
        Employee? employee = Get<Employee>($"{ControllerRoute}/{id}");
        return employee;
    }
}