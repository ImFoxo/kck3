using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteka.Models
{
    public class RentalBook
    {
        public RentalBook(int rentalId, int bookId, int? quantity)
        {
            this.rentalId = rentalId;
            this.bookId = bookId;
            this.quantity = quantity;
        }

        [Key,
            DatabaseGenerated(DatabaseGeneratedOption.Identity),
            Display(Name = "Id zamówionej książki"),
            Range(0, 9999999999)]
        public int rentalBookId { get; set; }

        public int rentalId { get; set; }
        public int bookId { get; set; }

        [ForeignKey("rentalId")]
        public virtual Rental rental { get; set; } 
        
        [ForeignKey("bookId")]
        public virtual Book book { get; set; }

        [BindProperty(SupportsGet = true),
            Required,
            Display(Name = "Ilość"),
            Range(0, 99999999, ErrorMessage = "Ilość musi być większa od zera oraz mniejsza niż 99999999")]
        public int? quantity { get; set; }
    }
}
