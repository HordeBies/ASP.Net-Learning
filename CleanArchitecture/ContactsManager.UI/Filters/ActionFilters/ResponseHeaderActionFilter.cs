using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        private string? key;
        private string? value;
        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order = 0, bool isReusable = false)
        {
            this.key = key;
            this.value = value;
            this.Order = order;
            this.IsReusable = isReusable;

        }
        public bool IsReusable { get; }

        public int Order { get; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var instance = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            instance.Key = key;
            instance.Value = value;
            return instance;
        }
    }
    public class ResponseHeaderActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> logger;
        public string? Key;
        public string? Value;
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogInformation("{ClassName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next();
            logger.LogInformation("{ClassName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
