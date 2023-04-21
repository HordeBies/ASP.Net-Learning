using Microsoft.AspNetCore.Mvc;
using ModelBindingExample.Models;
using System.Net;

namespace ModelBindingExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("ModelBindingExample -Home");
        }
        private IActionResult ValidateParams(bool? isloggedin,Book book)
        {
            if (!isloggedin.HasValue)
            {
                return Unauthorized("Authorization unidentified");
            }
            else if (isloggedin == false)
            {
                return Unauthorized("Forbidden request, not have authorization to access");

            }
            if (!book.BookId.HasValue)
            {
                return BadRequest("Book id is not supplied");
            }
            if (book.BookId <= 0)
            {
                return NotFound("Book id can't be less then or equal to zero");
            }
            if (book.BookId > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }
            return Content(
                $"IsLoggedIn: {isloggedin}\n" +
                $"Book: {book}", "text/plain");
        }
        //[Route("/bookstore/{bookid:int?}")]
        public IActionResult BookStore_ModelClass([FromQuery] int? bookid, [FromQuery] bool? isloggedin, Book book)
        {
            return ValidateParams(isloggedin,book);
        }
        [Route("/bookstore")]
        public IActionResult BookStore_FormFields(bool? isloggedin,Book book)
        {
            return ValidateParams(isloggedin,book);
        }
    }
}
