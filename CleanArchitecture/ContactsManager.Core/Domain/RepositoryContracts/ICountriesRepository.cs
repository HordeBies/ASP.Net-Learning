using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Country entity.
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new country to the data store.
        /// </summary>
        /// <param name="country">The country to add.</param>
        /// <returns>The added country with updated information, including the ID assigned by the data store.</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Retrieves a single country from the data store by ID.
        /// </summary>
        /// <param name="countryID">The ID of the country to retrieve.</param>
        /// <returns>The retrieved country information, or null if the country was not found.</returns>
        Task<Country?> GetCountry(Guid countryID);
        /// <summary>
        /// Retrieves a single country from the data store by Name.
        /// </summary>
        /// <param name="countryName">The name of the country to retrieve.</param>
        /// <returns>The retrieved country information, or null if the country was not found.</returns>
        Task<Country?> GetCountry(string countryName);

        /// <summary>
        /// Retrieves all countries from the data store.
        /// </summary>
        /// <returns>A list of all countries in the data store.</returns>
        Task<List<Country>> GetAllCountries();
    }
}