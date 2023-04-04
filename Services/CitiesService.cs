using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService
    {
        private List<string> _cities;
        public CitiesService()
        {
            _cities = new()
            {
                "London",
                "Paris",
                "New York",
                "Tokyo",
                "Rome"
            };
        }
        public List<string> GetCities()
        {
            return _cities;
        }
    }
}