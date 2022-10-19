using System.ComponentModel.DataAnnotations;

namespace Iliyan_Test_Identity_and_Entity_Framework_6.Models
{
    public class DisplayPlace
    {
        public int CountryId { get; set; }

        public int? CityId { get; set; }

        public string CountryName { get; set; }

        public string? CityName { get; set; }
    }
}
