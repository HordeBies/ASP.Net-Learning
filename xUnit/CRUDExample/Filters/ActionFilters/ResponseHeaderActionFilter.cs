using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> logger;
        private readonly string key;
        private readonly string value;
        public int Order { get; init; }
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
        {
            this.logger = logger;
            this.key = key;
            this.value = value;
            Order = order;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("{ClassName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuting));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("{ClassName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuted));

            context.HttpContext.Response.Headers.Add(key, value);
        }
    }
}
