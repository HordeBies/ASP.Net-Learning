using ServiceContracts;
using Services;
using Entities;
using RepositoryContracts;
using Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);

});

//Services
//builder.Services.AddScoped<ResponseHeaderActionFilter>(provider => new ResponseHeaderActionFilter(provider.GetRequiredService<ILogger<ResponseHeaderActionFilter>>(), "X-Custom-Key-Global", "Custom-Value-Global", 2));
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ResponseHeaderActionFilter>(5);
    //options.Filters.AddService<ResponseHeaderActionFilter>(2);
    options.Filters.Add(new ResponseHeaderActionFilter(builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>(), "X-Custom-Key-Global", "Custom-Value-Global", 2));
});

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonsConnection"));
});
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

var app = builder.Build();

if(builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }