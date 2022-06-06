using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Models;
using GB.ASPNET.WebStore.Interfaces;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index([FromServices]IProductData data)
        {
            ViewBag.Products = data
                .GetProducts()
                .OrderBy(el => el.Order)
                .Take(6)
                .ToViewmodels();
            return View();
        }

        public IActionResult ConfigString() => Content($"config: {_config["ServerGreetings"]}");
        public IActionResult ShowContent(string id = "-id-") => Content($"content: {id}");
    }
}