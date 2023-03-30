using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AssignmentLoginUsingMiddleware.Middlewares
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path != "/" || context.Request.Method != "POST")
            {
                context.Response.StatusCode = 200;
                return;
            }
            bool success = true;
            List<string> response = new List<string>();
            if (!context.Request.Query.ContainsKey("email"))
            {
                success = false;
                response.Add("Invalid input for 'email'\n");
            }
            if (!context.Request.Query.ContainsKey("password"))
            {
                success = false;
                response.Add("Invalid input for 'password'\n");
            }
            if (success)
            {
                string email = context.Request.Query["email"];
                string password = context.Request.Query["password"];
                if(email == "admin@example.com" && password == "admin1234")
                {
                    response.Add("Successful login\n");
                }
                else
                {
                    success = false;
                    response.Add("Invalid login\n");
                }
            }

            context.Response.StatusCode = success ? 200 : 400;
            foreach (var text in response)
            {
                await context.Response.WriteAsync(text);
            }

        }
    }

    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
