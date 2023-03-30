using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.CustomModelBinders;
using ModelValidationExample.Models;

namespace ModelValidationExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/register")]
        //[ModelBinder(typeof(PersonModelBinder))]
        //[Bind(new[] { nameof(Person.Password), nameof(Person.Name), nameof(Person.Email), nameof(Person.ConfirmPassword) })]
        public IActionResult Register([FromForm] Person person, [FromHeader(Name ="User-Agent")] string useragent)
        {
            //var errorList = new List<string>();
            //if (!ModelState.IsValid)
            //{
            //    foreach (var value in ModelState.Values)
            //    {
            //        foreach (var error in value.Errors)
            //        {
            //            errorList.Add(error.ErrorMessage);
            //        }
            //    }
            //    return BadRequest(string.Join("\n",errorList));
            //}
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
                return BadRequest(errors);
            }
            return Content($"{person}, {useragent}");
        }
    }
}
