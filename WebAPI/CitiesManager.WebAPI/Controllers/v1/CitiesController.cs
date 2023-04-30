using Microsoft.AspNetCore.Mvc;
using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.WebAPI.Controllers.v1
{
    /// <summary>
    /// Represents a controller for handling city-related API requests.
    /// </summary>
    [ApiVersion("1.0")]
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
        //[Produces("application/xml")] // adds header "Content-Type: application/xml" to this method's response only
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
        {
            return await citiesService.GetAllCities();
        }

        /// <summary>
        /// Retrieves a city by ID.
        /// </summary>
        /// <param name="id">The ID of the city to retrieve.</param>
        /// <returns>An action result containing the city response if found, otherwise returns an error response.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CityResponse>> GetCity(Guid id)
        {
            var city = await citiesService.GetCity(id);

            if (city == null)
            {
                return Problem(detail: $"Not found City with CityID: {id}", statusCode: 404, title: "Get City Failed");
                //return NotFound();
            }

            return city;
        }

        /// <summary>
        /// Updates an existing city.
        /// </summary>
        /// <param name="id">The ID of the city to update.</param>
        /// <param name="city">The city data to update with.</param>
        /// <returns>An action result containing the updated city response if successful, otherwise returns an error response.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CityResponse>> PutCity(Guid id, CityRequest city)
        {
            if (id != city.CityID)
            {
                return Problem(detail: "CityID from RouteParameter doesn't match CityID from request body", statusCode: 400, title: "Put City Failed");
                //return BadRequest();
            }

            var updatedCity = await citiesService.UpdateCity(city);

            return updatedCity;
        }

        /// <summary>
        /// Adds a new city.
        /// </summary>
        /// <param name="city">The city data to add.</param>
        /// <returns>An action result containing the created city response.</returns>
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(CityRequest city)
        {
            var addedCity = await citiesService.AddCity(city);

            return CreatedAtAction("GetCity", new { id = addedCity.CityID }, addedCity);
        }

        /// <summary>
        /// Deletes a city by ID.
        /// </summary>
        /// <param name="id">The ID of the city to delete.</param>
        /// <returns>An action result indicating success if the city was deleted, otherwise returns an error response.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var success = await citiesService.DeleteCity(id);
            if (!success)
            {
                return Problem(title: "Delete City Failed", detail: $"Unable to delete city with id {id}", statusCode: 404);
                //return NotFound();
            }

            return NoContent();
        }
    }

}
