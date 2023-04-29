using CitiesManager.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<City> Cities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(
                new City { CityID = Guid.Parse("D14BC922-C581-4181-B23C-5A86A6177C43"), CityName = "New York" },
                new City { CityID = Guid.Parse("8207DD6C-2504-46BA-9DC4-C2361F6F4E1F"), CityName = "London" },
                new City { CityID = Guid.Parse("8D9188E9-B249-4D2B-B564-1FE0E1997BC1"), CityName = "Paris" },
                new City { CityID = Guid.Parse("6BD9F2A9-3F22-47CB-999D-FDD1AB7790AA"), CityName = "Tokyo" },
                new City { CityID = Guid.Parse("9364A092-AAD7-48DC-9A71-4A4CF6E02211"), CityName = "Hong Kong" });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FirstBootCamp_CitiesDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

    }
}
