using ConfigurationExample.Options;
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
            var options = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();
            //var options = new WeatherApiOptions();
            //_configuration.GetSection("weatherapi").Bind(options);
            ViewBag.Key1 = options.ClientID;
            ViewBag.Key2 = options.ClientSecret;
            return View();
        }
    }
}
