using System;
using System.Collections.Generic;
using System.Text;

namespace BooksIO2026.Service.DTOs.Book
{
    public class BookDetailDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; }=null!;
        public string PublisherName { get; set; } = null!;
    }
}
