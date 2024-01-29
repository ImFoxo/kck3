using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Biblioteka.Models
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id wypożyczenia")]
        public int rentalId { get; set; }

        [BindProperty(SupportsGet = true),
            Display(Name = "Id użytkownika"),      
            Required(ErrorMessage = "Pole \"Id użytkownika\" jest wymagane!")]
        public int? userId { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Data wypożyczenia"),
            Required(ErrorMessage = "Pole \"Data wypożyczenia\" jest wymagane!")]
        public DateTime? rentalDate { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Stan wypożyczenia"),
            MaxLength(15, ErrorMessage = "Stan wypożyczenia nie może zawierać więcej niż 15 znaków"),
            Required(ErrorMessage = "Pole \"Stan wypożyczenia\" jest wymagane!")]
        public string? rentalState { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Data zmiany stanu"),
            Required(ErrorMessage = "Pole \"Data zmiany stanu\" jest wymagane!")]
        public DateTime? stateDate { get; set; }

        [BindProperty(SupportsGet = true),
            Display(Name = "PESEL"),
            Required(ErrorMessage = "Pole \"PESEL\" jest wymagane!"),
            RegularExpression(@"^\d{11}$", ErrorMessage = "Pole musi zawierać 11 cyfr.")]
        public string? PESEL { get; set; }

        [ForeignKey("userId"),
            Display(Name = "Użytkownik")]
        public virtual Reader? user { get; set; }

        public virtual ICollection<RentalBook>? RentalBook { get; set; }
    }
}
