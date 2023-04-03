using Microsoft.AspNetCore.Mvc;
using Services;

namespace DependencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly CitiesService _citiesService;
        public HomeController()
        {
            _citiesService = new CitiesService();
        }

        [Route("/")]
        public IActionResult Index()
        {
            var cities = _citiesService.GetCities();
            return View(cities);
        }
    }
}
