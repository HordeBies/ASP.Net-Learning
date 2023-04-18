using Entities;
using Microsoft.AspNetCore.Http;
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
        Task<CountryResponse> AddCountry(CountryAddRequest? request);

        /// <summary>
        /// Returns the list of all countries
        /// </summary>
        /// <returns>All countries from the list as List of <see cref="CountryResponse"/></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the countryID
        /// </summary>
        /// <param name="countryID">CountryID (guid) to search</param>
        /// <returns>Matching country as <see cref="CountryResponse"/> object</returns>
        Task<CountryResponse?> GetCountry(Guid? countryID);

        /// <summary>
        /// Uploads countries from an Excel file.
        /// </summary>
        /// <param name="formFile">The Excel file to upload.</param>
        /// <returns>The number of countries that were uploaded.</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}