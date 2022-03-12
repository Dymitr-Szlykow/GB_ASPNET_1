using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.ViewModels;
using GB.ASPNET.WebStore.Services;
using GB.ASPNET.WebStore.Services.Interfaces;

namespace GB.ASPNET.WebStore.Controllers;

//[Controller]
//[Route("staff/{action=Index}/{id?}")]
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
        IEnumerable<Employee> model = _employeesData.GetAll();
        return View("List", model);
    }

    public IActionResult List()
    {
        return View(_employeesData.GetAll());
    }

    //[Route("~/EmployeeInfo({id:int})")]
    public IActionResult Details(int id)
    {
        Employee? model = _employeesData.GetById(id);
        if (model == null) return NotFound();
        else return View(model);
    }

    public IActionResult Create()
    {
        return View("Update", new EmployeeVM());
    }

    public IActionResult Update(int? sentId)
    {
        if (sentId is not { } id) return View(new EmployeeVM());

        Employee? emp = _employeesData.GetById(id);
        if (emp is null) return NotFound();
        else
        {
            var model = new EmployeeVM
            {
                Id = emp.Id,
                NameFirst = emp.NameFirst,
                NameLast = emp.NameLast,
                NamePaternal = emp.NamePaternal,
                NameShort = emp.NameShort,
                Age = emp.Age
            };
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Update(EmployeeVM viewmodel)
    {
        if (viewmodel.NameLast == "Иванов" && viewmodel.Age < 21)
            ModelState.AddModelError(key: string.Empty, errorMessage: "Никаких Ивановых младше 21 года. Ну ладно, Иванову можно.");
        if (!ModelState.IsValid) return View(viewmodel);

        var employee = new Employee
        {
            Id = viewmodel.Id,
            NameFirst = viewmodel.NameFirst,
            NameLast = viewmodel.NameLast,
            NamePaternal = viewmodel.NamePaternal,
            Age = viewmodel.Age
        };
        if (employee.Id == 0)
        {
            var newId = _employeesData.Add(employee);
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

    public IActionResult Delete(int id)
    {
        if (id < 0) return BadRequest();
        Employee? emp = _employeesData.GetById((int)id);
        if (emp is null) return NotFound();
        else
        {
            var model = new EmployeeVM
            {
                Id = emp.Id,
                NameFirst = emp.NameFirst,
                NameLast = emp.NameLast,
                NamePaternal = emp.NamePaternal,
                NameShort = emp.NameShort,
                Age = emp.Age
            };
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult DeleteConfirm(int id)
    {
        if(!_employeesData.Delete(id)) return NotFound();
        else return RedirectToAction(nameof(List));
    }
}