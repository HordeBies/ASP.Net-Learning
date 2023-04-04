using Autofac;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DependencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService;
        private readonly IServiceScopeFactory _scopeFactory; //default
        private readonly ILifetimeScope _lifeTimeScope; //For autofac
        
        public HomeController(ICitiesService citiesService, IServiceScopeFactory scopeFactory, ILifetimeScope lifetimeScope)
        {
            _citiesService = citiesService;
            _scopeFactory = scopeFactory;
            _lifeTimeScope = lifetimeScope;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<string> cities;
            //using(var scope = _scopeFactory.CreateScope()) //default
            using(var scope = _lifeTimeScope.BeginLifetimeScope()) //For autofac
            {
                //Inject CitiesService
                //var citiesService = scope.ServiceProvider.GetService<ICitiesService>(); //default
                var citiesService = scope.Resolve<ICitiesService>(); //For autofac
                //DB work
                cities = _citiesService.GetCities();
                
            }//end of scope, it calls CitiesService.Dispose
            return View(cities);
        }
    }
}
