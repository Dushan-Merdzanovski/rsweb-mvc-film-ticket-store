using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCFilmTicketStore.Models
{
    public class Director : Person
    {
        public ICollection<Film>? Films { get; set; }
    }
}
