using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext db;
        public CountriesRepository(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
        }
        public async Task<Country> AddCountry(Country country)
        {
            db.Countries.Add(country);
            await db.SaveChangesAsync();
            return country;
        }
        public async Task<List<Country>> GetAllCountries()
        {
            return await db.Countries.ToListAsync();
        }
        public async Task<Country?> GetCountry(Guid countryID)
        {
            return await db.Countries.FirstOrDefaultAsync(c => c.CountryID.Equals(countryID));
        }
        public async Task<Country?> GetCountry(string countryName)
        {
            return await db.Countries.FirstOrDefaultAsync(c => c.CountryName != null && c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
