using Assignment_ModelBindingAndValidations.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_ModelBindingAndValidations.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost]
        [Route("/order")]
        public IActionResult GenerateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
                Response.StatusCode = 400;
                return Content(errors);
            }

            order.OrderNo = new Random().Next(1, 100000);
            
            return Json(order);
        }
    }
}
