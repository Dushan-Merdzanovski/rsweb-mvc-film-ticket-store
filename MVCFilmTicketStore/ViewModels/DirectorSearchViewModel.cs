using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.ViewModels
{
    public class DirectorSearchViewModel
    {
        public IList<Director> Directors { get; set; }
        public string SearchString { get; set; }
    }
}
