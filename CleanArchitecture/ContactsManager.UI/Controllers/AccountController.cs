using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request) 
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return View(request);
            }
            ApplicationUser user = new()
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.PersonName,
                PhoneNumber = request.Phone,
            };
            IdentityResult result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return View(request);
            }

            //Sign-In
            //TODO: Instead, redirect to Sign-In page to get isPersistent(Keep me signed in [x]) from user
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return View(request);
            }

            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, request.IsPersistent, false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("Login", "Invalid Email or Password.");
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return View(request);
            }

            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }
        //TODO: Seperate Get and Post and create a confirmation page
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }
    }
}
