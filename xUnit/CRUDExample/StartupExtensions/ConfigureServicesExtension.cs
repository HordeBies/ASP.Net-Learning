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
            
            //Persons Services
            services.AddScoped<IPersonsAdderService,PersonsAdderService>();
            services.AddScoped<IPersonsDeleterService,PersonsDeleterService>();
            
            //services.AddScoped<IPersonsGetterService,PersonsGetterService>();            
            //Both implementations work for OCP but PersonsGetterServiceChild_CompactExcel overrides the base class method and breaks the LSP principle therefore we prefer to use PersonsGetterService_CompactExcel
            //services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild_CompactExcel>();
            services.AddScoped<IPersonsGetterService, PersonsGetterService_CompactExcel>();

            services.AddScoped<IPersonsSorterService,PersonsSorterService>();
            services.AddScoped<IPersonsUpdaterService,PersonsUpdaterService>();

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
