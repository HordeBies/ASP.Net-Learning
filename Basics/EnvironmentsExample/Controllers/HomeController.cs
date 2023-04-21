using Microsoft.AspNetCore.Mvc;

namespace EnvironmentsExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("/")]
        [Route("/test")]
        public IActionResult Index()
        {
            if (_webHostEnvironment.IsDevelopment())
            {

            }
            ViewBag.CurrentEnvironment = _webHostEnvironment.EnvironmentName;
            return View();
        }
        [Route("/test")]
        public IActionResult Index2()
        {
            return View();
        }
    }
}
