using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Biblioteka.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id kategorii")]
        public int catId { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Nazwa"),
            MaxLength(20, ErrorMessage = "Nazwa nie może zawierać więcej niż 20 znaków"),
            Required(ErrorMessage = "Pole \"Nazwa\" jest wymagane!")]
        public string? catName { get; set; }
    }
}
