using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
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
            //TODO: Open db connection
        }


        public List<string> GetCities()
        {
            return _cities;
        }
        public void Dispose()
        {
            //TODO: Close db connection
        }
    }
}