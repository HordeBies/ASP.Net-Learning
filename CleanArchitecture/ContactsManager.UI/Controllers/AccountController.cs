using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO register) 
        { 
            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }
    }
}
