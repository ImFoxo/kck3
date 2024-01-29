using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Biblioteka.Models
{
    public class Employee
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

        [BindProperty(SupportsGet = true),
            Column(TypeName = "NUMERIC(11)"),
            Required(ErrorMessage = "Pole \"PESEL\" jest wymagane!"),
            Display(Name = "PESEL"),
            Range(10000000000, 99999999999, ErrorMessage = "PESEL składa się z 11 cyfr"),]
        public long? pesel { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Ulica\" jest wymagane!"),
            Display(Name = "Ulica"),
            MaxLength(40, ErrorMessage = "Ulica nie może zaweirać więcej niż 40 znaków")]
        public string? street { get; set; }

        [BindProperty(SupportsGet = true),
            Column(TypeName = "NUMERIC(3)"),
            Display(Name = "Nr mieszkania"),
            Range(0, 999, ErrorMessage = "Numer mieszkania nie może być większy niż 999")]
        public int? houseNumber { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Miasto\" jest wymagane!"),
            Display(Name = "Miasto"),
            MaxLength(30, ErrorMessage = "Miasto nie może zawierać więcej niż 30 znaków")]
        public string? town { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Kod pocztowy\" jest wymagane!"),
            Display(Name = "Kod pocztowy"),
            MaxLength(6, ErrorMessage = "Kod pocztowy nie może zawierać więcej niż 6 znaków")]
        public string? zipCode { get; set; }

        //[BindProperty(SupportsGet = true),
        //    Phone,
        //    Required,
        //    Column(TypeName = "NUMERIC(9)"),
        //    Display(Name = "Nr telefonu"),
        //    Range(100000000, 999999999)]
        //public string phoneNumber { get; set; }

        [BindProperty(SupportsGet = true),
            Required(ErrorMessage = "Pole \"Data zatrudnienia\" jest wymagane!"),
            Display(Name = "Data zatrudnienia")]
        public DateTime dateOfEmployment { get; set; }
    }
}
