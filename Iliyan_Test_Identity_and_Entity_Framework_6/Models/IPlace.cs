using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iliyan_Test_Identity_and_Entity_Framework_6.Models
{
    public interface IPlace
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
