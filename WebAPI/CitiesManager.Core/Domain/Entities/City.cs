using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.Domain.Entities
{
    public class City
    {
        [Key]
        public Guid CityID { get; set; }
        [Required]
        public string? CityName { get; set; }
    }
}
