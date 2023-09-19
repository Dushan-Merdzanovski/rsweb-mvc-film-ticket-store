using Microsoft.AspNetCore.Mvc.Rendering;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.ViewModels
{
    public class FilmGenreFilterViewModel
    {
        public IList<Film> Films { get; set; }
        public SelectList Genres { get; set; }
        public string FilterString { get; set; }
        public string SearchString { get; set; }
    }
}
