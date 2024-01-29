using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteka.Models
{
	public class Reader
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "Id autora")]
		public int id { get; set; }

		[BindProperty(SupportsGet = true),
			DisplayName("Imię"),
			Required(ErrorMessage = "Pole \"Imię\" jest wymagane!")]
		public string? name { get; set; }

		[BindProperty(SupportsGet = true),
			DisplayName("Nazwisko"),
			Required(ErrorMessage = "Pole \"Nazwisko\" jest wymagane!")]
		public string? surname { get; set; }

        [BindProperty(SupportsGet = true),
            Display(Name = "Adres e-mail"),
            EmailAddress,
            MaxLength(40, ErrorMessage = "E-mail nie może zawierać więcej niż 40 znaków"),
			Required(ErrorMessage = "Pole \"Adres e-mail\" jest wymagane!")]
        public string? email { get; set; }

        [BindProperty(SupportsGet = true),
			DisplayName("Data urodzenia"),
			DataType(DataType.Date, ErrorMessage = "Pole musi posiadać format daty"),
			Required(ErrorMessage = "Pole \"Data urodzenia\" jest wymagane!")]
		public DateTime? birthDate { get; set; }
	}
}
