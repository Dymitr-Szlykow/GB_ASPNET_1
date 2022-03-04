using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index() => View();

        public IActionResult ConfigString() => Content($"config: {_config["ServerGreetings"]}");
        public IActionResult ShowContent(string id = "-id-") => Content($"content: {id}");
    }
}
