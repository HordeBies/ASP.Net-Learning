using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DependencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService;
        public HomeController()
        {
            _citiesService = null;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var cities = _citiesService.GetCities();
            return View(cities);
        }
    }
}
