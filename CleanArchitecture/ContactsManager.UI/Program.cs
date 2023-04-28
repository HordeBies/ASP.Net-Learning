using ContactsManager.UI.Middlewares;
using ContactsManager.UI.StartupExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);

});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseHsts();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting(); // Identifying action method based on the route
app.UseAuthentication(); // Reading identity cookie
app.UseAuthorization(); // Validates access permissions of the user
app.MapControllers(); // Execute the filter pipeline (filters + action method itself)

// Conventional routing is not recommended for medium to large scale applications because it is not flexible enough to handle complex routing requirements, therefore we use attribute routing
app.UseEndpoints(endpoints => 
{
    endpoints.MapControllerRoute(name: "areas",pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "default",pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();

public partial class Program { }