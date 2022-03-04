using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Models;

namespace GB.ASPNET.WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> __employees = new List<Employee>()
        {
            new Employee { Id = 1, NameLast = "Иванов", NameFirst = "Иван", NamePaternal = "Иванович", Age = 23 },
            new Employee { Id = 2, NameLast = "Петров", NameFirst = "Петр", NamePaternal = "Петрович", Age = 27 },
            new Employee { Id = 3, NameLast = "Сидоров", NameFirst = "Сидор", NamePaternal = "Сидорович", Age = 18 }
        };

        public IActionResult Index() => View("List", __employees);
        public IActionResult List() => View(__employees);

        public IActionResult Details(int id)
        {
            return View(__employees.Find((element) => element.Id == id));
            Employee emp = __employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();
            else return View(emp);
        }

        public IActionResult Update(int? id, string? setNameFirst, string? setNameLast, string? setNamePaternal, int? setAge)
        {
            if (id != null)
            {
                var emp = __employees.Find((element) => element.Id == id);
                if (emp != null)
                {
                    if (setNameFirst != null) emp.NameFirst = setNameFirst;
                    if (setNameLast != null) emp.NameLast = setNameLast;
                    if (setNamePaternal != null) emp.NamePaternal = setNamePaternal;
                    if (setAge != null) emp.Age = (int)setAge;
                }
            }
            return View("List", __employees);
        }
        public IActionResult Employees() => View(__employees);
    }
}
