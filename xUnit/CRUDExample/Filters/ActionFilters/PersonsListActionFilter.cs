using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> logger;
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("{ClassName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuting));
            context.HttpContext.Items["args"] = context.ActionArguments;
            // validate searchBy parameter
            if (context.ActionArguments.TryGetValue("searchBy", out var value))
            {
                string? searchBy = Convert.ToString(value);
                if (!string.IsNullOrEmpty(searchBy))
                {
                        
                    var searchByOptions = typeof(PersonResponse).GetProperties().Select(p => p.Name).ToList();
                    
                    // reset searchBy to PersonName if it is not in the list
                    if (!searchByOptions.Any(s => s.Equals(searchBy, StringComparison.OrdinalIgnoreCase)))
                    {
                        logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("{ClassName}.{MethodName} method", nameof(PersonsListActionFilter),nameof(OnActionExecuted));
            var controller = (Controller)context.Controller;
            var args = context.HttpContext.Items["args"] as IDictionary<string,object?>;
            if(args != null)
            {
                object? value;
                if(args.TryGetValue("searchBy", out value))
                    controller.ViewData["searchBy"] = value?.ToString();
                if(args.TryGetValue("searchString", out value))
                    controller.ViewData["searchString"] = value?.ToString();
                if(args.TryGetValue("sortBy", out value))
                    controller.ViewData["sortBy"] = value?.ToString();
                else
                    controller.ViewData["sortBy"] = nameof(PersonResponse.PersonName);
                if(args.TryGetValue("sortOrder", out value))
                    controller.ViewData["sortOrder"] = value?.ToString();
                else
                    controller.ViewData["sortOrder"] = SortOrder.Ascending.ToString();
            }
            controller.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName),"Person Name" },
                { nameof(PersonResponse.Email),"Email" },
                { nameof(PersonResponse.Gender),"Gender" },
                { nameof(PersonResponse.DateOfBirth),"Date Of Birth" },
                { nameof(PersonResponse.Country),"Country" },
                { nameof(PersonResponse.Address),"Address" },
            };

        }
    }
}
