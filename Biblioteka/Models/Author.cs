using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Id autora")]
        public int id { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Imię"),
            Required(ErrorMessage ="Pole \"Imię\" jest wymagane!")]
        public string? name { get; set; }
        
        [BindProperty(SupportsGet = true),
            DisplayName("Nazwisko"),
            Required(ErrorMessage ="Pole \"Nazwisko\" jest wymagane!")]
        public string? surname { get; set; }
        
        [BindProperty(SupportsGet = true),
            DisplayName("Kraj"),
            Required(ErrorMessage ="Pole \"Kraj\" jest wymagane!")]
        public string? country { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Data urodzenia"),
            DataType(DataType.Date, ErrorMessage ="Pole musi posiadać format daty"),
            Required(ErrorMessage = "Pole \"Data urodzenia\" jest wymagane!")]
        public DateTime? date { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Pseudonim")]
        public string? alias { get; set; }

        [BindProperty(SupportsGet = true),
           Display(Name = "Zdjęcie (wklej adres URL)")]
        public string? authorPhoto { get; set; }

        [ForeignKey("Book"),
            Display(Name = "Książki")]
        public List<Book>? books { get; set; }

    }
}
