using Microsoft.AspNetCore.Mvc.Rendering;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.ViewModels
{
    public class ActorSearchViewModel
    {
        public IList<Actor> Actors { get; set; }
        public string SearchString { get; set; }
    }
}
