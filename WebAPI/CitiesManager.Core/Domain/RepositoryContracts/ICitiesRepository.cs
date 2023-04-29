using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.Domain.RepositoryContracts
{
    public interface ICitiesRepository
    {
        /// <summary>
        /// Adds a new city to the data store.
        /// </summary>
        /// <param name="city">The city to add.</param>
        /// <returns>The added city with updated information, including the ID assigned by the data store.</returns>
        Task<City> AddCity(City city);

        /// <summary>
        /// Retrieves a single city from the data store by ID.
        /// </summary>
        /// <param name="cityID">The ID of the city to retrieve.</param>
        /// <returns>The retrieved city information, or null if the city was not found.</returns>
        Task<City?> GetCity(Guid cityID);

        /// <summary>
        /// Retrieves a single city from the data store by name.
        /// </summary>
        /// <param name="cityName">The name of the city to retrieve.</param>
        /// <returns>The retrieved city information, or null if the city was not found.</returns>
        Task<City?> GetCity(string cityName);

        /// <summary>
        /// Updates an existing city in the data store.
        /// </summary>
        /// <param name="city">The city to update.</param>
        /// <returns>The updated city information.</returns>
        Task<City> UpdateCity(City city);

        /// <summary>
        /// Deletes an existing city from the data store.
        /// </summary>
        /// <param name="cityID">The ID of the city to delete.</param>
        /// <returns>True if the city was deleted, false if it was not found.</returns>
        Task<bool> DeleteCity(Guid cityID);

        /// <summary>
        /// Retrieves all cities from the data store.
        /// </summary>
        /// <returns>A list of all cities in the data store.</returns>
        Task<List<City>> GetAllCities();
    }
}
