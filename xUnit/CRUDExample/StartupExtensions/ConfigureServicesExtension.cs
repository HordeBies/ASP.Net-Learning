using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.ResultFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add<ResponseHeaderActionFilter>(5);
                options.Filters.Add(new ResponseHeaderFilterFactoryAttribute("X-Custom-Key-Global", "Custom-Value-Global", 2));
                //options.Filters.AddService<ResponseHeaderActionFilter>(2);
            });
            //Filter Services
            //builder.Services.AddScoped<ResponseHeaderActionFilter>(provider => new ResponseHeaderActionFilter(provider.GetRequiredService<ILogger<ResponseHeaderActionFilter>>(), "X-Custom-Key-Global", "Custom-Value-Global", 2));
            services.AddTransient<ResponseHeaderActionFilter>();
            services.AddTransient<PersonsListActionFilter>();
            services.AddTransient<PersonsListResultFilter>();

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsService, PersonsService>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PersonsConnection"));
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
