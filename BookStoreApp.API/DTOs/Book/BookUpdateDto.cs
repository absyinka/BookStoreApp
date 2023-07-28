using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.DTOs.Book
{
    public class BookUpdateDto : BaseDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        public int Year { get; set; }

        [Required]
        public string Isbn { get; set; } = null!;

        public string? Summary { get; set; }

        public string? Image { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int AuthorId { get; set; }
    }
}
