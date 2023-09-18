using System.ComponentModel.DataAnnotations;

namespace MVCFilmTicketStore.Models
{
    public class Theater
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Film Theater")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "All seats")]
        public int AllSeatsNum { get; set; }

        [Required]
        [Display(Name = "Rows")]
        public int AllRows { get; set; }

        [Required]
        [Display(Name = "Rows")]
        public int AllColumns { get; set; }

        [MaxLength(100)]
        public string? TheaterPicture { get; set; }

        public ICollection<Projection>? Projections { get; set; } 
    }
}
