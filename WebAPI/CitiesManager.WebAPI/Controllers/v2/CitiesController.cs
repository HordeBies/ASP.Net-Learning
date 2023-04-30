using Microsoft.AspNetCore.Mvc;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.WebAPI.Controllers.v2
{
    /// <summary>
    /// Represents a controller for handling city-related API requests.
    /// </summary>
    [ApiVersion("2.0")]
    public class CitiesController : ApiControllerBase
    {
        private readonly ICitiesService citiesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesController"/> class.
        /// </summary>
        /// <param name="citiesService">The service used for handling city-related data.</param>
        public CitiesController(ICitiesService citiesService)
        {
            this.citiesService = citiesService;
        }

        /// <summary>
        /// Retrieves all cities.
        /// </summary>
        /// <returns>An action result containing a list of city responses.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string?>>> GetCities()
        {
            return (await citiesService.GetAllCities()).Select(r => r.CityName).ToList();
        }

    }

}
