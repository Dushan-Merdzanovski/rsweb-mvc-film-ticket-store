using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.ViewModels
{
    public class ReserveSeatsViewModel
    {
        public Projection? Projection { get; set; }
        public Ticket? NewTicket { get; set; }

        public List<Seat>? TakenSeats { get; set; }

        public IList<Seat>? AllSeats { get; set; }
    }
}
