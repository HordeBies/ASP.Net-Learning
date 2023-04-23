using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonRedirectPostActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonRedirectPostActionFilter> logger;
        private readonly ICountriesService countriesService;
        public PersonRedirectPostActionFilter(ILogger<PersonRedirectPostActionFilter> logger, ICountriesService countriesService)
        {
            this.logger = logger;
            this.countriesService = countriesService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogInformation("{ClassName}.{MethodName} method - before", nameof(PersonRedirectPostActionFilter), nameof(OnActionExecutionAsync));

            if (!context.ModelState.IsValid && context.Controller is PersonsController controller)
            {
                controller.ViewBag.Countries = (await countriesService.GetAllCountries()).Select(c => new SelectListItem(c.CountryName, c.CountryID.ToString()));
                controller.ViewBag.Errors = controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                context.Result = controller.View(context.ActionArguments["request"]);
            }
            else
            {
                await next();
                logger.LogInformation("{ClassName}.{MethodName} method - after", nameof(PersonRedirectPostActionFilter), nameof(OnActionExecutionAsync));
            }
        }
    }

}
