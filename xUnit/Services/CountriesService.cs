using Entities;
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
        public CountryResponse AddCountry(CountryAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Request is null");
            }
            if (string.IsNullOrEmpty(request.CountryName))
            {
                throw new ArgumentException("Country name is null or empty");
            }
            if(db.Countries.Any(c=> c.CountryName == request.CountryName))
            {
                throw new ArgumentException("Country name already exists");
            }
            var country = request.ToCountry(); 
            country.CountryID = Guid.NewGuid();
            db.Countries.Add(country);
            db.SaveChanges();
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetCountries()
        {
            var countries = db.Countries.ToList();
            return countries.Select(c=> c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountry(Guid? countryID)
        {
            if (countryID == null)
                return null;
            return db.Countries.FirstOrDefault(c => c.CountryID == countryID)?.ToCountryResponse();
        }
    }
}