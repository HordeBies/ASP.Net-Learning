using Microsoft.AspNetCore.Mvc;
using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            this.citiesService = citiesService;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
        {
            return await citiesService.GetAllCities();
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityResponse>> GetCity(Guid id)
        {
            var city = await citiesService.GetCity(id);

            if (city == null)
            {
                return Problem(detail:$"Not found City with CityID: {id}",statusCode:404,title: "Get City Failed");
                //return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CityResponse>> PutCity(Guid id, CityRequest city)
        {
            if (id != city.CityID)
            {
                return Problem(detail:"CityID from RouteParameter doesn't match CityID from request body",statusCode:400,title: "Put City Failed");
                //return BadRequest();
            }

            var updatedCity = await citiesService.UpdateCity(city);

            return updatedCity;
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(CityRequest city)
        {
            var addedCity = await citiesService.AddCity(city);

            return CreatedAtAction("GetCity", new { id = addedCity.CityID }, addedCity);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var success = await citiesService.DeleteCity(id);
            if (!success)
            {
                return Problem(title: "Delete City Failed", detail: $"Unable to delete city with id {id}",statusCode: 404);
                //return NotFound();
            }

            return NoContent();
        }
    }
}
