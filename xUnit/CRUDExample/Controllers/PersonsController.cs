using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            this.personsService = personsService;
            this.countriesService = countriesService;
        }

        [Route("/")]
        [Route("/persons/index")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName),SortOrder sortOrder = SortOrder.Ascending)
        {
            var model = personsService.GetPersons();
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName),"Person Name" },
                { nameof(PersonResponse.Email),"Email" },
                { nameof(PersonResponse.Gender),"Gender" },
                { nameof(PersonResponse.DateOfBirth),"Date Of Birth" },
                { nameof(PersonResponse.Country),"Country" },
                { nameof(PersonResponse.Address),"Address" },
            };
            model = personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.searchBy = searchBy;
            ViewBag.searchString = searchString;
            model = personsService.GetSortedPersons(model, sortBy, sortOrder);
            ViewBag.sortBy = sortBy;
            ViewBag.sortOrder = sortOrder.ToString();
            return View(model);
        }
        [Route("/persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = countriesService.GetCountries();
            return View();
        }
        [Route("/persons/create")]
        [HttpPost]
        public IActionResult Create([FromForm]PersonAddRequest request)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Countries = countriesService.GetCountries();
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            var response = personsService.AddPerson(request);
            return RedirectToAction("Index","Persons");
        }
    }
}
