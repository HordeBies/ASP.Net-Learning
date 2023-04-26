using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> logger;
        private readonly bool isDisabled;
        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
        {
            this.logger = logger;
            this.isDisabled = isDisabled;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            logger.LogInformation("{ClassName}.{MethodName} method - before", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));

            if (isDisabled)
            {
                context.Result = new StatusCodeResult(503); //404 Not Found, 501 Not Implemented, 503 Service Unavailable
            }
            if (context.Result == null)
                await next();

            logger.LogInformation("{ClassName}.{MethodName} method - after", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
        }
    }
}
