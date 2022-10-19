using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iliyan_Test_Identity_and_Entity_Framework_6.Models
{
    public class Country : IPlace
    {
        [Key]
        public int Id { get; set; }

        //Set the maximum letters to 56 because it fits the number of letters of the longest country name in the world.
        [Column(TypeName ="nvarchar(56)")]
        public string Name { get; set; }

        public List<City>? Cities { get; set; }
    }
}
