using RoutingExample.Constraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("days", typeof(DaysConstraint));
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}.{extension?}", async (context) =>
    {
        await context.Response.WriteAsync("In Files\n");
        await context.Response.WriteAsync(context.Request.RouteValues["filename"]?.ToString()+"\n");
        await context.Response.WriteAsync(context.Request.RouteValues["extension"]?.ToString()+"\n");
    });
    endpoints.Map("employee/profile/{name:alpha=demirci}",async context =>
    {
        await context.Response.WriteAsync("In Employee Profile\n");
        await context.Response.WriteAsync($"Profile of {context.Request.RouteValues["name"]?.ToString()}");
    });
    endpoints.Map("products/{id:int=1}", async context =>
    {
        await context.Response.WriteAsync("In Products\n");
        await context.Response.WriteAsync($"Details of {context.Request.RouteValues["id"]?.ToString()}");
    });
    endpoints.Map("weather/{day:days}", async context =>
    {
        await context.Response.WriteAsync("In Weather\n");
        await context.Response.WriteAsync($"{context.Request.RouteValues["day"]?.ToString()}");
    });

});

app.Run();
