using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        List<Person> people = new()
            {
                new Person {Name = "Ali",Age = 13},
                new Person {Name = "Hasan", Age = 14},
                new Person {Name = "Mehmet", Age =15},
            };
        [Route("/")]
        [Route("/home")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "Ali Türkübar";

            //ViewData["people"] = people;
            return View(people);
        }
        [Route("PersonDetails/{name?}")]
        public IActionResult Details(string? name)
        {
            if(name == null)
            {
                return BadRequest("Person Name can't be null");
            }
            var person = people.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());
            if(person == null)
            {
                return BadRequest($"No Person with name: {name} found in collection");
            }
            return View(person);
        }
        [Route("/test")]
        public IActionResult Test()
        {
            return View();
        }
    }
}
