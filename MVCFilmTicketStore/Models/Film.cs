using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace MVCFilmTicketStore.Models
{
    public class Film
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Film Duration")]
        public TimeSpan FilmDuration { get; set; }

        public string? Decription { get; set; }

        public string? Poster { get; set; }

        [Display(Name = "Download Poster")]
        public string? DownloadPosterUrl { get; set; }



        [Display(Name = "Director")]
        public int DirectorId { get; set; }
        public Director? Director { get; set; }

        public ICollection<ActorFilm>? ActorFilms { get; set; }

        public ICollection<Review>? Reviews{ get; set; }

        public ICollection<FilmGenre>? FilmGenres { get; set; }

        public ICollection<Projection>? Projections { get; set; }

    }
}
