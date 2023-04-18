using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
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
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName),SortOrder sortOrder = SortOrder.Ascending)
        {
            var model = await personsService.GetAllPersons();
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName),"Person Name" },
                { nameof(PersonResponse.Email),"Email" },
                { nameof(PersonResponse.Gender),"Gender" },
                { nameof(PersonResponse.DateOfBirth),"Date Of Birth" },
                { nameof(PersonResponse.Country),"Country" },
                { nameof(PersonResponse.Address),"Address" },
            };
            model = await personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.searchBy = searchBy;
            ViewBag.searchString = searchString;
            model = await personsService.GetSortedPersons(model, sortBy, sortOrder);
            ViewBag.sortBy = sortBy;
            ViewBag.sortOrder = sortOrder.ToString();
            return View(model);
        }
        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName,c.CountryID.ToString()));

            return View();
        }
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]PersonAddRequest request)
        {
            if (!ModelState.IsValid) //client side validation makes this part obsolete
            {
                ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            var response = await personsService.AddPerson(request);
            return RedirectToAction("Index","Persons");
        }
        [HttpGet]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Edit(Guid PersonID)
        {
            var person = await personsService.GetPerson(PersonID);
            if (person == null)
                return RedirectToAction("Index");
            ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));
            return View(person.ToPersonUpdateRequest());
        }
        [HttpPost]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest person)
        {
            await personsService.UpdatePerson(person);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Delete(Guid PersonID)
        {
            var person = await personsService.GetPerson(PersonID);
            if(person == null)
                return RedirectToAction("Index");
            return View(person);
        }

        [HttpPost]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Delete(PersonResponse person)
        {
            var success = await personsService.DeletePerson(person.PersonID);
            //TempData["MessageType"] = success ? "Deleted" : "PersonNotFound";
            //TempData["MessageText"] = success ? "Successfully deleted " + person.PersonName : "Could not found " + person.PersonName;
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            var persons = await personsService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new(20,20,20,20),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            var stream = await personsService.GetPersonsCSV();
            return File(stream, "application/octet-stream","persons.csv");
        }
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            var stream = await personsService.GetPersonsExcel();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
