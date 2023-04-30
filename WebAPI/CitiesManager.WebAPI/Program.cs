using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using CitiesManager.Infrastructure.DbContexts;
using CitiesManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json")); // adds header "Content-Type: application/json" to all responses
    options.Filters.Add(new ConsumesAttribute("application/json")); // adds header "Content-Type: application/json" to all requests
});

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
    //config.ApiVersionReader = new QueryStringApiVersionReader(); // from query string ?api-version=1.0
    //config.ApiVersionReader = new HeaderApiVersionReader(); // from header "api-version: 1.0"
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

//Custom Services
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICitiesService, CitiesService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer(); // generates description for all endpoints
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CitiesManager.WebAPI.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cities Web API", Version = "1.0" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Cities Web API", Version = "2.0" });
}); // generates OpenAPI specification
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger(); // creates endpoint for swagger.json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
}); // creates swagger UI for testing all Web API endpoints / action methods
app.UseAuthorization();

app.MapControllers();

app.Run();
