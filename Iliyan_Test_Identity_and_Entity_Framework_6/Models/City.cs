using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iliyan_Test_Identity_and_Entity_Framework_6.Models
{
    public class City : IPlace
    {
        [Key]
        public int Id { get; set; }

        //Set the maximum letters to 163 because it fits the number of letters of the longest city name in the world.
        [Column(TypeName = "nvarchar(163)")]
        public string Name { get; set; }

        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
    }
}
