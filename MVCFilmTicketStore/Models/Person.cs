using MVCFilmTicketStore.DataTypes.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MVCFilmTicketStore.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nationality")]
        public string? Country { get; set; }

        public Gender? Gender { get; set; }

        public string? ProfilePictureUrl { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        [NotMapped]
        public int Age
        {
            get 
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now.Date - BirthDate;
                return (zeroTime + span).Year - 1;
            }
        }
    }
}
