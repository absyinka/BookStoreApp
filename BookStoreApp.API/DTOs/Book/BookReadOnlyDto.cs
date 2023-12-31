﻿using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.DTOs.Book
{
    public class BookReadOnlyDto : BaseDto
    {
        public string Title { get; set; } = null!;

        public string? Image { get; set; }

        public decimal Price { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; } = null!;
    }
}
