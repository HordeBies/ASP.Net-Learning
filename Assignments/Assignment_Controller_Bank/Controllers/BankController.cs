using Microsoft.AspNetCore.Mvc;

namespace Assignment_Controller_Bank.Controllers
{
    public class BankController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank", "text/plain");
        }
        [Route("/account-details")]
        public IActionResult GetAccountDetails()
        {
            return Json(new { accountNumber = 1001, accountHolderName = "Example Name", currentBalance = 5000 });
        }
        [Route("/account-statement")]
        public IActionResult GetAccountStatement()
        {
            return File("/sample.pdf","application/pdf");
        }
        [Route("/get-current-balance/{accountnumber:int?}")]
        public IActionResult GetCurrentBalance() 
        {
            if(!Request.RouteValues.ContainsKey("accountnumber"))
            {
                return NotFound("Account Number should be supplied");
            }
            var accountNumber = Convert.ToInt32(Request.RouteValues["accountnumber"]);
            
            if(accountNumber != 1001)
            {
                return BadRequest("Account Number should be 1001");
            }

            return Content("5000","text/plain");
        }
    }
}
