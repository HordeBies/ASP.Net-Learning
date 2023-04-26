using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.UI.Filters;
using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.AuthorizationFilters;
using ContactsManager.UI.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace ContactsManager.UI.Controllers
{
    [Route("Persons")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key-Controller", "Custom-Value-Controller", 3}, Order = 3)]
    [ResponseHeaderFilterFactory("X-Custom-Key-Controller", "Custom-Value-Controller", 3)]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly ICountriesService countriesService;
        private readonly ILogger<PersonsController> logger;
        public PersonsController(ICountriesService countriesService, ILogger<PersonsController> logger)
        {
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
        public async Task<IActionResult> Index([FromServices] IPersonsGetterService personsGetterService, [FromServices] IPersonsSorterService personsSorterService, string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrder sortOrder = SortOrder.Ascending)
        {
            logger.LogInformation("Index action method is called");
            logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");

            var model = await personsGetterService.GetFilteredPersons(searchBy, searchString);
            //ViewBag.searchBy = searchBy;
            //ViewBag.searchString = searchString;
            model = await personsSorterService.GetSortedPersons(model, sortBy, sortOrder);
            //ViewBag.sortBy = sortBy;
            //ViewBag.sortOrder = sortOrder.ToString();
            return View(model);
        }
        [HttpGet]
        [Route("create")]
        //[TypeFilter(typeof(FeatureDisabledResourceFilter))]
        public async Task<IActionResult> Create()
        {
            ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));

            return View();
        }
        [HttpPost]
        [Route("create")]
        [TypeFilter(typeof(PersonRedirectPostActionFilter))]
        public async Task<IActionResult> Create([FromServices] IPersonsAdderService personsAdderService, [FromForm] PersonAddRequest request)
        {
            var response = await personsAdderService.AddPerson(request);
            return RedirectToAction("Index", "Persons");
        }
        [HttpGet]
        [Route("[action]/{PersonID}")]
        //[TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit([FromServices] IPersonsGetterService personsGetterService, Guid PersonID)
        {
            var person = await personsGetterService.GetPerson(PersonID);
            if (person == null)
                return RedirectToAction("Index");
            ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));
            return View(person.ToPersonUpdateRequest());
        }
        [HttpPost]
        [Route("[action]/{PersonID}")]
        [TypeFilter(typeof(PersonRedirectPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit([FromServices] IPersonsUpdaterService personsUpdaterService, PersonUpdateRequest request)
        {
            await personsUpdaterService.UpdatePerson(request);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Delete([FromServices] IPersonsGetterService personsGetterService, Guid PersonID)
        {
            var person = await personsGetterService.GetPerson(PersonID);
            if (person == null)
                return RedirectToAction("Index");
            return View(person);
        }

        [HttpPost]
        [Route("[action]/{PersonID}")]
        public async Task<IActionResult> Delete([FromServices] IPersonsDeleterService personsDeleterService, PersonResponse person)
        {
            var success = await personsDeleterService.DeletePerson(person.PersonID);
            //TempData["MessageType"] = success ? "Deleted" : "PersonNotFound";
            //TempData["MessageText"] = success ? "Successfully deleted " + person.PersonName : "Could not found " + person.PersonName;
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF([FromServices] IPersonsGetterService personsGetterService)
        {
            var persons = await personsGetterService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new(20, 20, 20, 20),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV([FromServices] IPersonsGetterService personsGetterService)
        {
            var stream = await personsGetterService.GetPersonsCSV();
            return File(stream, "application/octet-stream", "persons.csv");
        }
        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel([FromServices] IPersonsGetterService personsGetterService)
        {
            var stream = await personsGetterService.GetPersonsExcel();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
