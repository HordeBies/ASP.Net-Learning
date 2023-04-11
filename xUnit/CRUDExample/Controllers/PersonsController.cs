using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
    }
}
