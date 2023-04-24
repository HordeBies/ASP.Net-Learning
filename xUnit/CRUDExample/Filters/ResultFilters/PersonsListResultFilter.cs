using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> logger;
        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            logger.LogInformation("{ClassName}.{MethodName} method - before", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));
            context.HttpContext.Response.Headers["X-Last-Modified"] = DateTime.Now.ToString("U");

            await next();
            logger.LogInformation("{ClassName}.{MethodName} method - after", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

        }
    }
}
