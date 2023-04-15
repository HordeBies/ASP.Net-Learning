using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext db;
        public CountriesService(PersonsDbContext personsDbContext)
        {
            db = personsDbContext;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request is null");
            }
            if (string.IsNullOrEmpty(request.CountryName))
            {
                throw new ArgumentException("Country name is null or empty");
            }
            if(await db.Countries.AnyAsync(c=> c.CountryName == request.CountryName))
            {
                throw new ArgumentException("Country name already exists");
            }
            var country = request.ToCountry(); 
            country.CountryID = Guid.NewGuid();
            db.Countries.Add(country);
            await db.SaveChangesAsync();
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetCountries()
        {
            var countries = await db.Countries.ToListAsync();
            return countries.Select(c=> c.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountry(Guid? countryID)
        {
            if (countryID == null)
                return null;
            var country = await db.Countries.FirstOrDefaultAsync(c => c.CountryID == countryID);
            return country?.ToCountryResponse();
        }
    }
}