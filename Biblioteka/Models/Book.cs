using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Biblioteka.Models
{
    public class Book
    {
        [Key,
            DatabaseGenerated(DatabaseGeneratedOption.Identity),
            Display(Name = "Id książki"),
            Range(0, 9999)]
        public int bookId { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Tytuł\" jest wymagane!"),
            Display(Name = "Tytuł"),
            MaxLength(50, ErrorMessage = "Tytuł nie może zawierać więcej niż 50 znaków")]
        public string? title { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Numer ISBN\" jest wymagane!"),
            Display(Name = "Numer ISBN"),
            Range(1000000000000, 9999999999999, ErrorMessage = "Numer ISBN składa się z 13 cyfr")]
        public long? ISBN { get; set; }

        [BindProperty(SupportsGet = true),
        Required(ErrorMessage = "Pole \"Opis\" jest wymagane!"),
            Display(Name = "Opis"),
            MaxLength(400, ErrorMessage = "Opis nie może zawierać więcej niż 400 znaków")]
        public string? description { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Data wydania\" jest wymagane!"),
            Display(Name = "Data wydania"),
            DataType(DataType.Date, ErrorMessage = "Pole musi posiadać format daty")]
        public DateTime? releaseDate { get; set; }

        [ForeignKey("Author"),
            Display(Name = "Autorzy")]
        public List<Author>? authors { get; set; }

        [ForeignKey("Tag"),
            Display(Name = "Tagi")]
        public List<Tag>? tags { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Stan magazynowy\" jest wymagane!"),
            Display(Name = "Stan magazynowy"),
            Range(0, 99999999, ErrorMessage = "Stan magazynowy musi być większy od zera oraz mniejszy niż 99999999")]
        public int? stockLevel { get; set; }

        [BindProperty(SupportsGet = true),       
            Display(Name = "Zdjęcie (wklej adres URL)")]
        public string? bookPhoto { get; set; }

        public int catId { get; set; }

        [BindProperty(SupportsGet = true),
            Display(Name = "PDF do pobrania"),
            NotMapped]
        public IFormFile file { get; set; }

        public string? fileAsString { get; set; }

        [ForeignKey("catId"),
            Display(Name = "Kategoria")]
        public virtual Category? category { get; set; }


        public virtual ICollection<RentalBook>? RentalBook { get; set; }
    }
}
