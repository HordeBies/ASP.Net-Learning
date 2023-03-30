using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareExample.CustomMiddleware
{
    public class MySecondCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MySecondCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("My Second Custom Middleware -Start\n");
            await _next(context);
            await context.Response.WriteAsync("My Second Custom Middleware -End\n");
        }
    }

    public static class MySecondCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMySecondCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MySecondCustomMiddleware>();
        }
    }
}
