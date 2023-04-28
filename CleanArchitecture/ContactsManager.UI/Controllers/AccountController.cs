using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public static string ControllerName => "Account";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request, string? ReturnUrl) 
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
            // Role Assignment
            if (!await roleManager.RoleExistsAsync(request.UserType.ToString()))
                await roleManager.CreateAsync(new ApplicationRole { Name = request.UserType.ToString() });
            await userManager.AddToRoleAsync(user, request.UserType.ToString());

            //Sign-In
            //TODO: Instead, redirect to Sign-In page to get isPersistent(Keep me signed in [x]) from user
            await signInManager.SignInAsync(user, isPersistent: false);
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return LocalRedirect(ReturnUrl);
            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
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
            if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return LocalRedirect(ReturnUrl);
            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }
        //TODO: Seperate Get and Post and create a confirmation page
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), PersonsController.ControllerName);
        }

        public async Task<IActionResult> IsEmailNotRegistered(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user != null)
                return Json($"Email {email} is already registered.");
            return Json(true); 
        }
    }
}
