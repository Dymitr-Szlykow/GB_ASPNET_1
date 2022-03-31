using GB.ASPNET.WebStore.DAL.Context;
using GB.ASPNET.WebStore.Domain;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Services.InSQL;

public class SqlEmployeeData : IEmployeesData
{
    private readonly WebStoreDB _dbContext;
    private readonly ILogger<SqlProductData> _logger;

    public SqlEmployeeData(WebStoreDB dbContext, ILogger<SqlProductData> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public int? Add(Employee employee)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public bool Edit(Employee employee)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Employee> GetAll()
    {
        return _dbContext.Employees;
    }

    public Employee? GetById(int id)
    {
        return _dbContext.Employees.Find(id);
    }
}