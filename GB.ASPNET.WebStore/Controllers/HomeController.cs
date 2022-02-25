using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Models;

namespace GB.ASPNET.WebStore.Controllers
{
    //[Controller]
    public class HomeController : Controller
    {
        private static readonly List<Employee> __employees = new List<Employee>()
        {
            new Employee { Id = 1, NameLast = "Иванов", NameFirst = "Иван", NamePaternal = "Иванович", Age = 23 },
            new Employee { Id = 2, NameLast = "Петров", NameFirst = "Петр", NamePaternal = "Петрович", Age = 27 },
            new Employee { Id = 3, NameLast = "Сидоров", NameFirst = "Сидор", NamePaternal = "Сидорович", Age = 18 }
        };

        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            this._config = config;
        }

        public IActionResult Index() => View();

        public IActionResult ConfigString() => Content($"config: {_config["ServerGreetings"]}");
        public IActionResult EmployeeDetails(int id) => View(__employees.Find( (element) => element.Id == id));
        public IActionResult EmployeeUpdate(int? id, string? setNameFirst, string? setNameLast, string? setNamePaternal, int? setAge)
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
            return View("Employees", __employees);
        }
        public IActionResult Employees() => View(__employees);
        public IActionResult ShowContent(string id = "-id-") => Content($"content: {id}");
    }
}
