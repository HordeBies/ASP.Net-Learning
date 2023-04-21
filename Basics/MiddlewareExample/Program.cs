
using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyCustomMiddleware>();

var app = builder.Build();

app.Use(async (HttpContext context,RequestDelegate next) =>{
    await context.Response.WriteAsync("Middleware 1\n");
    await next(context);
});

app.Use(async (context, next) => {
    await context.Response.WriteAsync("Middleware 2\n");
    await next(context);
});

//app.UseMiddleware<MyCustomMiddleware>();
app.UseMyCustomMiddleware();
app.UseMySecondCustomMiddleware();
app.UseWhen(
    context => context.Request.Query.ContainsKey("name"),
    app =>{
        app.Use(async (context, next) =>{
            await context.Response.WriteAsync($"Hello {context.Request.Query["name"]}\n");
            await next(context);
        });
    });
app.Run(async (context) => {
    await context.Response.WriteAsync("Middleware 3\n");
});

app.Run();
