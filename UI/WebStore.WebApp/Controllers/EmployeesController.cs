using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Domain.Entities.Identity;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    private readonly IEmployeesData _employeesData;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger)
    {
        _employeesData = employeesData;
        _logger = logger;
    }


    public IActionResult Index()
    {
        var listViewmodel = new List<EmployeeVM>();
        foreach (Employee employee in _employeesData.GetAll())
            listViewmodel.Add(employee.ToViewmodel()!);

        return View("List", listViewmodel);
    }

    public IActionResult List()
    {
        var listViewmodel = new List<EmployeeVM>();
        foreach (Employee employee in _employeesData.GetAll())
            listViewmodel.Add(employee.ToViewmodel()!);

        return View(listViewmodel);
    }

    public IActionResult Details(int id)
    {
        Employee? model = _employeesData.GetById(id);
        if (model == null) return NotFound();
        else return View(model.ToViewmodel());
    }

    [Authorize(Roles = Role.administrators)]
    public IActionResult Create()
    {
        return View("Update", new EmployeeVM());
    }

    [Authorize(Roles = Role.administrators)]
    public IActionResult Update(int? sentId)
    {
        if (sentId is null) return View(new EmployeeVM());

        Employee? emp = _employeesData.GetById((int)sentId);
        if (emp is null) return NotFound();
        else return View(emp.ToViewmodel());
    }

    [HttpPost]
    [Authorize(Roles = Role.administrators)]
    public IActionResult Update(EmployeeVM viewmodel)
    {
        if (viewmodel.NameLast == "Иванов" && viewmodel.Age < 21)
            ModelState.AddModelError(key: string.Empty, errorMessage: "Никаких Ивановых младше 21 года. Ну ладно, Ивановой можно.");
        if (!ModelState.IsValid) return View(viewmodel);

        Employee employee = viewmodel?.ToEntityModel()!;
        if (employee.Id == 0)
        {
            int? newId = _employeesData.Add(employee);
            return RedirectToAction(nameof(List), _employeesData);
            //return RedirectToAction(nameof(Details), newId);
            //return RedirectToRoute($"~/EmployeeInfo({newId})");
            //return RedirectToRoute($"~/Employees/Details/{newId}");
        }
        else
        {
            _employeesData.Edit(employee);
            return RedirectToAction(nameof(List), _employeesData);
                //View("List", _employeesData.GetAll());
        }
    }

    [Authorize(Roles = Role.administrators)]
    public IActionResult Delete(int id)
    {
        if (id < 0) return BadRequest();
        Employee? emp = _employeesData.GetById(id);
        if (emp is null) return NotFound();
        else return View(emp.ToViewmodel());
    }

    [HttpPost]
    [Authorize(Roles = Role.administrators)]
    public IActionResult DeleteConfirm(int id)
    {
        if(!_employeesData.Delete(id)) return NotFound();
        else return RedirectToAction(nameof(List));
    }
}