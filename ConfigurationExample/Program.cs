using ConfigurationExample.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Host.ConfigureAppConfiguration((hostingContext,config) =>
{
    config.AddJsonFile("CustomConfig.json", optional: true, reloadOnChange: true);
    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    config.AddEnvironmentVariables();
    config.AddCommandLine(args);
});

builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherapi"));
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
