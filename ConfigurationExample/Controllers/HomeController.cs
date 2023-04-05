using Microsoft.AspNetCore.Mvc;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("/")]
        public IActionResult Index()
        {
            //ViewBag.Key1 = _configuration["weatherapi:clientid"];
            //ViewBag.Key2 = _configuration["weatherapi:clientsecret"];
            var weatherapi = _configuration.GetSection("weatherapi");
            ViewBag.Key1 = weatherapi["clientid"];
            ViewBag.Key2 = weatherapi["clientsecret"];
            return View();
        }
    }
}
