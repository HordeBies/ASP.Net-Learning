using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Home","text/plain");
        }
        [Route("/bookstore")]
        public IActionResult BookStore()
        {
            if (!Request.Query.ContainsKey("isloggedin"))
            {
                return Unauthorized("Authorization unidentified");
            }
            else if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
            {
                return Unauthorized("Forbidden request, not have authorization to access");

            }
            if (!Request.Query.ContainsKey("bookid"))
            {
                return BadRequest("Book id is not supplied");
            }
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                return BadRequest("Book id can't be null or empty");
            }
            int bookId = Convert.ToInt32(Request.Query["bookid"]);
            if(bookId <= 0)
            {
                return NotFound("Book id can't be less then or equal to zero");
            }
            if(bookId > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            return RedirectToActionPermanent(actionName:"Book",controllerName:"Store");
        }
    }
}
