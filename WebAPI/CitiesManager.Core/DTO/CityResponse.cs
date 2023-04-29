using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.DTO
{
    public class CityResponse
    {
        public Guid CityID { get; set; }
        public string? CityName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CityResponse)) return false;
            var other = (CityResponse)obj;
            return CityID == other.CityID &&
                CityName == other.CityName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class CityResponseExtensions
    {
        public static CityResponse ToCityResponse(this City city)
        {
            return new CityResponse()
            {
                CityID = city.CityID,
                CityName = city.CityName
            };
        }
    }
}
