using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;
using ContactsManager.Core.Helpers;

namespace CitiesManager.Core.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly ICitiesRepository citiesRepository;
        public CitiesService(ICitiesRepository citiesRepository)
        {
            this.citiesRepository = citiesRepository;
        }

        public async Task<CityResponse> AddCity(CityRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var city = request.ToCity();
            city = await citiesRepository.AddCity(city);

            return city.ToCityResponse();
        }

        public async Task<bool> DeleteCity(Guid? cityID)
        {
            if (cityID == null || cityID == Guid.Empty) 
                return false;
            var city = await citiesRepository.GetCity(cityID.Value);
            if(city == null) return false;
            await citiesRepository.DeleteCity(cityID.Value);
            return true;
        }

        public async Task<List<CityResponse>> GetAllCities()
        {
            return (await citiesRepository.GetAllCities()).Select(c => c.ToCityResponse()).ToList();
        }

        public async Task<CityResponse?> GetCity(Guid? cityID)
        {
            if (cityID == null || cityID == Guid.Empty)
                return null;
            var city = await citiesRepository.GetCity(cityID.Value);
            if(city == null) return null;
            return city.ToCityResponse();
        }

        public async Task<CityResponse?> GetCity(string? cityName)
        {
            if(string.IsNullOrWhiteSpace(cityName))
                return null;
            var city = await citiesRepository.GetCity(cityName);
            if(city == null) return null;
            return city.ToCityResponse();
        }

        public async Task<CityResponse> UpdateCity(CityRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ModelValidation(request);
            var match = await citiesRepository.GetCity(request.CityID);
            if(match == null) throw new ArgumentException("Given city does not exist");
            match.CityName = request.CityName;
            match = await citiesRepository.UpdateCity(match);
            return match.ToCityResponse();
        }
    }
}
