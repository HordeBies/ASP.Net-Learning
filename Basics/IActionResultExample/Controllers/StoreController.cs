using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class StoreController : Controller
    {
        [Route("/store/book")]
        public IActionResult Book()
        {
            return File("/sample.pdf", "application/pdf");
        }
    }
}
