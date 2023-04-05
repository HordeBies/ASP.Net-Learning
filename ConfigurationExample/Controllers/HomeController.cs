using ConfigurationExample.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherApiOptions _options;
        public HomeController(IOptions<WeatherApiOptions> options)
        {
            _options = options.Value;
        }
        [Route("/")]
        public IActionResult Index()
        {
            //ViewBag.Key1 = _configuration["weatherapi:clientid"];
            //ViewBag.Key2 = _configuration["weatherapi:clientsecret"];
            //var options = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();
            //var options = new WeatherApiOptions();
            //_configuration.GetSection("weatherapi").Bind(options);
            ViewBag.Key1 = _options.ClientID;
            ViewBag.Key2 = _options.ClientSecret;
            return View();
        }
    }
}
