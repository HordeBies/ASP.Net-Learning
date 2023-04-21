using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("/home")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }
        [Route("/friends-list")]
        public IActionResult LoadFriendsList()
        {
            PersonGridModel model = new PersonGridModel()
            {
                GridTitle = "Osmanlı Evlatları",
                People = new()
                {
                    new Person() {PersonName = "Lfbee", JobTitle = "Senato 1"},
                    new Person() {PersonName = "Bies", JobTitle = "Senato 2"},
                    new Person() {PersonName = "Ya3", JobTitle = "Senato 3"},
                }
            };
            return ViewComponent("Grid", new { model });
        }
    }
}
