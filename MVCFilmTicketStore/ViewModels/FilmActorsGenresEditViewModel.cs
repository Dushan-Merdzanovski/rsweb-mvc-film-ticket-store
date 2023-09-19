using Microsoft.AspNetCore.Mvc.Rendering;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.ViewModels
{
    public class FilmActorsGenresEditViewModel
    {
        public Film Film { get; set; }

        public IEnumerable<int>? SelectedActors { get; set; }
        public IEnumerable<SelectListItem>? ActorList { get; set; }

        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
