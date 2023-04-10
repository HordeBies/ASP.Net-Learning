using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService personsService;
        public PersonsController(IPersonsService personsService)
        {
            this.personsService = personsService;
        }

        [Route("/")]
        [Route("/persons/index")]
        public IActionResult Index()
        {
            var model = personsService.GetPersons();
            return View(model);
        }
    }
}
