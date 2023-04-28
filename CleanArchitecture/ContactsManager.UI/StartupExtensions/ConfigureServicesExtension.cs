using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.DbContexts;
using ContactsManager.Infrastructure.Repositories;
using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.ResultFilters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.UI.StartupExtensions
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
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();

            //services.AddScoped<IPersonsGetterService,PersonsGetterService>();            
            //Both implementations work for OCP but PersonsGetterServiceChild_CompactExcel overrides the base class method and breaks the LSP principle therefore we prefer to use PersonsGetterService_CompactExcel
            //services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild_CompactExcel>();
            services.AddScoped<IPersonsGetterService, PersonsGetterService_CompactExcel>();

            services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();

            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PersonsConnection"));
            });

            services.AddIdentity<ApplicationUser,ApplicationRole>(options =>
            {
                // Default Password complexity
                //options.Password.RequiredLength = 6;
                //options.Password.RequireDigit = true;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredUniqueChars = 1;

                // Reduced complexity for ease of use
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 1;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
