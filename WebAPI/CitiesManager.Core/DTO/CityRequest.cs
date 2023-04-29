using CitiesManager.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO
{
    public class CityRequest
    {
        [Required]
        public Guid CityID { get; set; }
        public string? CityName { get; set; }

        public City ToCity()
        {
            return new() { CityID = this.CityID, CityName = this.CityName };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CityRequest)) return false;
            var other = (CityRequest)obj;
            return CityID == other.CityID &&
                CityName == other.CityName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
