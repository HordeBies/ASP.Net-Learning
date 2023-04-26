using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.Any(f => f is SkipFilter))
                return;
            context.HttpContext.Response.Headers.Add("X-Always-Added", "Always Added");
        }
    }
}
