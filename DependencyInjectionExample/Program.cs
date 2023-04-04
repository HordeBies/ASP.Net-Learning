using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllersWithViews();
//builder.Services.Add(new(
//    typeof(ICitiesService),
//    typeof(CitiesService),
//    ServiceLifetime.Scoped));
//builder.Services.AddTransient<ICitiesService, CitiesService>();
//builder.Services.AddScoped<ICitiesService, CitiesService>();
//builder.Services.AddSingleton<ICitiesService, CitiesService>();
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => //Autofac as IoC
{
    //containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency(); //Transient
    containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope(); //Scoped
    //containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance(); //Singleton
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
