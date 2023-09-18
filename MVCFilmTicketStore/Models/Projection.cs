using System.ComponentModel.DataAnnotations;

namespace MVCFilmTicketStore.Models
{
    public class Projection
    {
        public int Id { get; set; }

        [Display(Name = "Number Of Free Seats")]
        public int FreeSeatsNum { get; set; }

        [Required]
        [Display(Name = "Projection Time")]
        public DateTime ProjectionTime { get; set; }

        [Required]
        [Display(Name = "Price")]
        public float Price { get; set; }

        [Display(Name = "3D")]
        public bool Is3D { get; set; }

        public int FilmId { get; set; }
        public Film? Film { get; set; }

        public int TheaterId { get; set; }
        public Theater? Theater { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
