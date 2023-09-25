using Microsoft.AspNetCore.Mvc.Rendering;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.ViewModels
{
    public class BuyTicketViewModel
    {
        public Projection? Projection { get; set; }
        public Ticket? NewTicket { get; set; }
    }
}
