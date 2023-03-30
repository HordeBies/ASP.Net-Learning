using Microsoft.AspNetCore.Mvc;

namespace LayoutViewsExample.Controllers
{
    public class ProductsController : Controller
    {
        [Route("/products")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("products/search")]
        public IActionResult Search()
        {
            return View();
        }
        [Route("products/order")]
        public IActionResult Order()
        {
            return View();
        }
    }
}
