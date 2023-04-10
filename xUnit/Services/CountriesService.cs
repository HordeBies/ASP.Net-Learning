using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> countries;
        public CountriesService(bool initialize = true)
        {
            countries = new List<Country>();
            if(initialize)
            {
                countries.Add(new Country() { CountryID = Guid.Parse("AC1556E1-1811-4294-8049-D6C2352D2E73"), CountryName = "USA" });
                countries.Add(new Country() { CountryID = Guid.Parse("16F464AD-65D2-4E82-BD51-BDE0FEE91BB8"), CountryName = "Canada" });
                countries.Add(new Country() { CountryID = Guid.Parse("7DB3A34C-5B12-4558-93D2-997318B0E95A"), CountryName = "UK" });
                countries.Add(new Country() { CountryID = Guid.Parse("E839C350-555B-4847-A3A2-43517D543FC6"), CountryName = "India" });
                countries.Add(new Country() { CountryID = Guid.Parse("2A638819-BEC1-4EEC-81EE-D864CEE07C46"), CountryName = "Turkey" });
            }
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

        public CountryResponse? GetCountry(Guid? countryID)
        {
            if (countryID == null)
                return null;

            return countries.FirstOrDefault(c => c.CountryID == countryID)?.ToCountryResponse();
        }
    }
}