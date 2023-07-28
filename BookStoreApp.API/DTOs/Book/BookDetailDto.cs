namespace BookStoreApp.API.DTOs.Book
{
    public class BookDetailDto : BaseDto
    {
        public string Title { get; set; } = null!;

        public int Year { get; set; }

        public string Isbn { get; set; } = null!;

        public string? Summary { get; set; }

        public string? Image { get; set; }

        public decimal Price { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; } = null!;
    }
}
