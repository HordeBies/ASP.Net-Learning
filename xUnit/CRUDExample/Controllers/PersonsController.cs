using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("Persons")]
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
        [Route("index")]
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
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = countriesService.GetCountries().Select(c => new SelectListItem(c.CountryName,c.CountryID.ToString()));

            return View();
        }
        [Route("create")]
        [HttpPost]
        public IActionResult Create([FromForm]PersonAddRequest request)
        {
            if (!ModelState.IsValid) //client side validation makes this part obsolete
            {
                ViewBag.Countries = countriesService.GetCountries().Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            var response = personsService.AddPerson(request);
            return RedirectToAction("Index","Persons");
        }
        [HttpGet]
        [Route("[action]/{PersonID}")]
        public IActionResult Edit(Guid PersonID)
        {
            var person = personsService.GetPerson(PersonID);
            if (person == null)
                return RedirectToAction("Index");
            ViewBag.Countries = countriesService.GetCountries().Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));
            return View(person.ToPersonUpdateRequest());
        }
        [HttpPost]
        [Route("[action]/{PersonID}")]
        public IActionResult Edit(PersonUpdateRequest person)
        {
            personsService.UpdatePerson(person);
            return RedirectToAction("Index", "Persons");
        }
    }
}
