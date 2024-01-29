using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Biblioteka.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id tagu")]
        public int tagId { get; set; }

        [BindProperty(SupportsGet = true),
            DisplayName("Nazwa"),
            MaxLength(20, ErrorMessage = "Nazwa nie może zawierać więcej niż 20 znaków"),
            Required(ErrorMessage = "Pole \"Nazwa\" jest wymagane!")]
        public string? tagName { get; set; }

        [ForeignKey("Book"),
            Display(Name = "Książki")]
        public List<Book>? books { get; set; }
    }
}
