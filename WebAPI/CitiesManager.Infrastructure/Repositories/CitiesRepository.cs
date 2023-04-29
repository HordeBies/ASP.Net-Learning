using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.Repositories
{
    public class CitiesRepository : ICitiesRepository
    {
        private readonly ApplicationDbContext db;
        public CitiesRepository(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        public async Task<City> AddCity(City city)
        {
            db.Cities.Add(city);
            await db.SaveChangesAsync();
            return city;
        }

        public async Task<List<City>> GetAllCities()
        {
            return await db.Cities.ToListAsync();
        }

        public async Task<City?> GetCity(Guid cityID)
        {
            return await db.Cities.FirstOrDefaultAsync(c => c.CityID.Equals(cityID));
        }

        public async Task<City?> GetCity(string cityName)
        {
            return await db.Cities.FirstOrDefaultAsync(c => cityName.Equals(c.CityName));
        }

        public async Task<City> UpdateCity(City city)
        {
            var match = await db.Cities.FirstOrDefaultAsync(c => c.CityID.Equals(city.CityID));
            if (match == null)
                return city;

            match.CityName = city.CityName;

            await db.SaveChangesAsync();
            return match;
        }

        public async Task<bool> DeleteCity(Guid cityID)
        {
            var city = await db.Cities.FindAsync(cityID);
            if (city != null)
            {
                db.Cities.Remove(city);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
