var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();




Dictionary<int,string> countries = new() { { 1, "United States" },{ 2, "Canada" },{ 3, "United Kingdom" },{ 4, "India" },{ 5, "Japan" } };
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/countries",async context =>
    {
        foreach (var kvp in countries)
        {
            await context.Response.WriteAsync($"{kvp.Key},{kvp.Value}\n");
        }
    });
    endpoints.MapGet("/countries/{countryID:int:range(0,100)}", async context =>
    {
        int id = Convert.ToInt16(context.Request.RouteValues["countryID"]);
        if (countries.ContainsKey(id))
        {
            await context.Response.WriteAsync(countries[id]);
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("[No Country]");
        }
    });
    endpoints.MapGet("/countries/{countryID:int:min(101)}", async context =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
    });
});

app.Run();