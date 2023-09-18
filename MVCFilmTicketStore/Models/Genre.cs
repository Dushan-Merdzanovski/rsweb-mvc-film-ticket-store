using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCFilmTicketStore.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Genre")]
        public string GenreName { get; set; }

        public ICollection<FilmGenre>? FilmGenres { get; set; }
    }
}
