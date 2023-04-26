﻿using Serilog;

namespace ContactsManager.UI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        private readonly IDiagnosticContext diagnostic;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IDiagnosticContext diagnosticContext)
        {
            this.next = next;
            this.logger = logger;
            this.diagnostic = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    logger.LogError("{ExceptionType} {ExceptionMessage}", e.InnerException.GetType().ToString(), e.InnerException.Message);
                else
                    logger.LogError("{ExceptionType} {ExceptionMessage}", e.GetType().ToString(), e.Message);

                //httpContext.Response.StatusCode = 500;
                //await httpContext.Response.WriteAsync("Error occured");

                throw;
            }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
