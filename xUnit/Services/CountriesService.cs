using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> countries;
        public CountriesService()
        {
            countries = new List<Country>();
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
            if(countries.Any(c=> c.CountryName == request.CountryName))
            {
                throw new ArgumentException("Country name already exists");
            }
            var country = request.ToCountry(); 
            country.CountryID = Guid.NewGuid();
            countries.Add(country);
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetCountries()
        {
            return countries.Select(c=> c.ToCountryResponse()).ToList();
        }
    }
}