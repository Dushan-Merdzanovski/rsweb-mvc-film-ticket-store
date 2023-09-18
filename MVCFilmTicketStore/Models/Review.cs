using System.ComponentModel.DataAnnotations;

namespace MVCFilmTicketStore.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string AppUser { get; set; }

        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        public int FilmId { get; set; }
        public Film? Film { get; set; }
    }
}
