using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DependencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService;
        private readonly IServiceScopeFactory _scopeFactory;
        public HomeController(ICitiesService citiesService, IServiceScopeFactory scopeFactory)
        {
            _citiesService = citiesService;
            _scopeFactory = scopeFactory;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities;
            using(var scope = _scopeFactory.CreateScope())
            {
                //Inject CitiesService
                var citiesService = scope.ServiceProvider.GetService<ICitiesService>();
                //DB work
                cities = _citiesService.GetCities();
                
            }//end of scope, it calls CitiesService.Dispose
            return View(cities);
        }
    }
}
