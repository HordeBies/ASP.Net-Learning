using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.AuthorizationFilters
{
    public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ILogger<TokenAuthorizationFilter> logger;
        public TokenAuthorizationFilter(ILogger<TokenAuthorizationFilter> logger)
        {
            this.logger = logger;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            logger.LogInformation("{ClassName}.{MethodName} method", nameof(TokenAuthorizationFilter), nameof(OnAuthorizationAsync));

            if (context.HttpContext.Request.Cookies.TryGetValue("Auth-Key", out var token) && token == "ABC12345")
            {
                //Success
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
