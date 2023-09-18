using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCFilmTicketStore.Models
{
    public class ActorFilm
    {
        public int Id { get; set; }

        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
        public int FilmId { get; set; }
        public Film? Film { get; set; }
    }
}
