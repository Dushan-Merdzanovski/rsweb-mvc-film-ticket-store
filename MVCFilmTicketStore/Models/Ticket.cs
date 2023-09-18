using System.ComponentModel.DataAnnotations;

namespace MVCFilmTicketStore.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Display(Name = "Time Purchased")]
        public DateTime BoughtTime { get; set; }

        [MaxLength(100)]
        public string? DownloadTicketUrl { get; set; }

        [Display(Name = "Valid Ticket")]
        public bool? IsValid { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string AppUser { get; set; }

        [Display(Name = "Projection")]
        public int ProjectionId { get; set; }
        public Projection? Projection { get; set; }
        public ICollection<TicketSeat>? TicketSeats { get; set; }
    }
}
