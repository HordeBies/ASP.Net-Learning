using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilters;
using CRUDExample.Filters.ExceptionFilters;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("Persons")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key-Controller", "Custom-Value-Controller", 3}, Order = 3)]
    [ResponseHeaderFilterFactory("X-Custom-Key-Controller", "Custom-Value-Controller", 3)]
    [TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonsService personsService;
        private readonly ICountriesService countriesService;
        private readonly ILogger<PersonsController> logger;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            this.personsService = personsService;
            this.countriesService = countriesService;
            this.logger = logger;
        }

        [Route("/")]
        [Route("index")]
        [ServiceFilter(typeof(PersonsListActionFilter), Order = 4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key-Action", "Custom-Value-Action", 1}, Order = 1)]
        [ResponseHeaderFilterFactory("X-Custom-Key-Action", "Custom-Value-Action", 1)]
        [ServiceFilter(typeof(PersonsListResultFilter))]
        [SkipFilter]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName),SortOrder sortOrder = SortOrder.Ascending)
        {
            logger.LogInformation("Index action method is called");
            logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");

            var model = await personsService.GetFilteredPersons(searchBy, searchString);
            //ViewBag.searchBy = searchBy;
            //ViewBag.searchString = searchString;
            model = await personsService.GetSortedPersons(model, sortBy, sortOrder);
            //ViewBag.sortBy = sortBy;
            //ViewBag.sortOrder = sortOrder.ToString();
            return View(model);
        }
        [HttpGet]
        [Route("create")]
        //[TypeFilter(typeof(FeatureDisabledResourceFilter))]
        public async Task<IActionResult> Create()
        {
            ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName,c.CountryID.ToString()));

            return View();
        }
        [HttpPost]
        [Route("create")]
        [TypeFilter(typeof(PersonRedirectPostActionFilter))]
        public async Task<IActionResult> Create([FromForm]PersonAddRequest request)
        {
            var response = await personsService.AddPerson(request);
            return RedirectToAction("Index","Persons");
        }
        [HttpGet]
        [Route("[action]/{PersonID}")]
        //[TypeFilter(typeof(TokenResultFilter))]
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
        [TypeFilter(typeof(PersonRedirectPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest request)
        {
            await personsService.UpdatePerson(request);
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
