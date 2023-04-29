﻿using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Country entity.
    /// </summary>
    public interface ICountriesRepository
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
        /// Retrieves all cities from the data store.
        /// </summary>
        /// <returns>A list of all cities in the data store.</returns>
        Task<List<City>> GetAllCities();


    }

}