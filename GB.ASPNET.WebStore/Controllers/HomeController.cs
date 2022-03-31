using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Services.Interfaces;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Controllers
{
    //[Controller]
    public class HomeController : Controller
    {

        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            this._config = config;
        }

        public IActionResult Index([FromServices]IProductData data)
        {
            ViewBag.Products = data
                .GetProducts()
                .OrderBy(el => el.Order)
                .Take(6)
                .Select(el => new ProductViewModel()
                {
                    Id = el.Id,
                    Name = el.Name,
                    ImageUrl = el.ImageUrl,
                    Price = el.Price,
                });
            return View();
        }

        public IActionResult ConfigString() => Content($"config: {_config["ServerGreetings"]}");
        public IActionResult ShowContent(string id = "-id-") => Content($"content: {id}");
    }
}
