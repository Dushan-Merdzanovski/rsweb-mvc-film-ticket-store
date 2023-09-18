using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCFilmTicketStore.Models
{
    public class Actor : Person
    {
        public ICollection<ActorFilm>? ActorFilms {  get; set; }
    }
}