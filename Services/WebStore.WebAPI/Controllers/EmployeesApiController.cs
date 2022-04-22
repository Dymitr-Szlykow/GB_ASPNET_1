using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _dataManipulator;
    private readonly ILogger<EmployeesApiController> _logger;

    public EmployeesApiController(IEmployeesData dataService, ILogger<EmployeesApiController> logger)
    {
        _dataManipulator = dataService;
        _logger = logger;
    }


    [HttpPost]                      // POST /localhost:5001/api/employees HTTP/1.1
    [HttpPost("add")]               // POST /localhost:5001/api/employees/add HTTP/1.1
    [HttpPost("create")]            // POST /localhost:5001/api/employees/create HTTP/1.1
    public IActionResult Create([FromBody] Employee model)
    {
        int? newId = _dataManipulator.Add(model);
        if (newId > 0)
        {
            _logger.LogInformation("Запись сотрудника с id={newId} создана: {model}", newId, model);
            return CreatedAtAction(nameof(GetById), new { newId }, model);
        }
        else
        {
            _logger.LogError("Ошибка создания записи сотрудника: {model}", model);
            return Problem();
        }
    }


    [HttpGet]                       // GET /localhost:5001/api/employees HTTP/1.1
    public IActionResult GetAll()
    {
        IEnumerable<Employee>? result = _dataManipulator.GetAll();
        return result.Any() ? Ok(result) : NoContent();
    }


    [HttpGet("{id:int}")]           // GET /localhost:5001/api/employees/42 HTTP/1.1
    public IActionResult GetById(int id)
    {
        Employee? employee = _dataManipulator.GetById(id);
        return employee is not null ? Ok(employee) : NotFound(new { id });
    }


    [HttpDelete("{id:int}")]        // DELETE /localhost:5001/api/employees/42 HTTP/1.1
    [HttpDelete("delete/{id:int}")] // DELETE /localhost:5001/api/employees/delete/42 HTTP/1.1
    public IActionResult Delete(int id)
    {
        if (_dataManipulator.Delete(id))
        {
            _logger.LogInformation("Запись сотрудника с id={id} удалена.", id);
            return Ok(true);
        }
        else
        {
            _logger.LogWarning("Попытка удаления записи сотрудника с id={id}: не найдена.", id);
            return NotFound(false);
        }
    }


    [HttpPut]                       // PUT /localhost:5001/api/employees HTTP/1.1
    [HttpPut("update")]             // PUT /localhost:5001/api/employees/update HTTP/1.1
    [HttpPut("edit")]               // PUT /localhost:5001/api/employees/edit HTTP/1.1
    public IActionResult Update([FromBody] Employee model)
    {
        if (_dataManipulator.Edit(model))
        {
            _logger.LogInformation("Запись сотрудника с id={Id} обновлена: {model}.", model.Id, model);
            return Ok(true);
        }
        else
        {
            _logger.LogError("Ошибка обновления записи сотрудника с id={Id}: {model}.", model.Id, model);
            return NotFound(false);
        }
    }
}