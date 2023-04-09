using Entities;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entities
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="request">Country object to add</param>
        /// <returns>Returns the country object after adding it</returns>
        DTO.CountryResponse AddCountry(CountryAddRequest? request);
        /// <summary>
        /// Returns the list of all countries
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</returns>
        List<CountryResponse> GetCountries();

        /// <summary>
        /// Returns a country object based on the countryID
        /// </summary>
        /// <param name="countryID">CountryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        public CountryResponse? GetCountry(Guid? countryID);
    }
}