using CitiesManager.Core.DTO;

namespace CitiesManager.Core.ServiceContracts
{
    public interface ICitiesService
    {
        /// <summary>
        /// Adds a city object to the list of cities
        /// </summary>
        /// <param name="request">City object to add</param>
        /// <returns>Returns the city object after adding it</returns>
        Task<CityResponse> AddCity(CityRequest? request);

        /// <summary>
        /// Returns the list of all cities
        /// </summary>
        /// <returns>All cities from the list as List of <see cref="CityResponse"/></returns>
        Task<List<CityResponse>> GetAllCities();

        /// <summary>
        /// Returns a city object based on the cityID
        /// </summary>
        /// <param name="cityID">CityID (guid) to search</param>
        /// <returns>Matching city as <see cref="CityResponse"/> object</returns>
        Task<CityResponse?> GetCity(Guid? cityID);

        /// <summary>
        /// Returns a city object based on the City Name
        /// </summary>
        /// <param name="cityName">City Name (string) to search</param>
        /// <returns>Matching city as <see cref="CityResponse"/> object</returns>
        Task<CityResponse?> GetCity(string? cityName);

        /// <summary>
        /// Updates an existing city in the list of cities
        /// </summary>
        /// <param name="request">The updated city object</param>
        /// <returns>The updated city object</returns>
        Task<CityResponse> UpdateCity(CityRequest request);

        /// <summary>
        /// Deletes a city object from the list of cities based on cityID
        /// </summary>
        /// <param name="cityID">CityID (guid) to delete</param>
        /// <returns>True if the city was deleted, false if it was not found.</returns>
        Task<bool> DeleteCity(Guid? cityID);

    }
}
