using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> logger;
        private readonly IHostEnvironment env;
        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.env = hostEnvironment;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogError("Exception filter {FilterName}.{MethodName}\n\t{ExceptionType}\n\t{ExceptionMessage}", nameof(HandleExceptionFilter), nameof(OnException), context.Exception.GetType().ToString(), context.Exception.Message);

            if (env.IsDevelopment())
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 500,
                    Content = context.Exception.Message
                };
            }
            else
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 500,
                    Content = "Internal Server Error"
                };
            }
        }
    }
}
