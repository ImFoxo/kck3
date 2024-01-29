using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteka.Models
{
    public class AdminSettings 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id ustawień")]
        public int adminSettingsId { get; set; }

        [BindProperty(SupportsGet = true),
            Required,
            Display(Name = "Limit przetrzymywanych książek"),
            Range(0, 11, ErrorMessage = "Limit musi być większy od 0 i mniejszy od 11")]
        public int? limitTaken { get; set; }

        [BindProperty(SupportsGet = true),
            Required,
            Display(Name = "Limit książek oczekujących na odbiór"),
            Range(0, 11, ErrorMessage = "Limit musi być większy od 0 i mniejszy od 11")]
        public int? limitWaiting { get; set; }

        [BindProperty(SupportsGet = true),
            Required,
            Display(Name = "Limit CZASOWY przetrzymywanych książek (ilość tygodni)"),
            Range(0, 48, ErrorMessage = "Limit musi być większy od 0 i mniejszy od 48")]
        public int? limitTimeTaken { get; set; }

        [BindProperty(SupportsGet = true),
            Required,
            Display(Name = "Limit CZASOWY książek oczekujących na odbiór (ilość tygodni)"),
            Range(0, 48, ErrorMessage = "Limit musi być większy od 0 i mniejszy od 48")]
        public int? limitTimeWaiting { get; set; }
              
    }
}