using System.ComponentModel.DataAnnotations;

namespace MVCFilmTicketStore.Models
{
    public class Seat
    {
        public int Id { get; set; }

        [Required]
        public int Row { get; set; }

        [Required]
        public int Column { get; set; }

        public ICollection<TicketSeat>? TicketSeats { get; set; }
    }
}
