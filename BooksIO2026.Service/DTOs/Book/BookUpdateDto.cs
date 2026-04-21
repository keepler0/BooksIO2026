namespace BooksIO2026.Service.DTOs.Book
{
    public class BookUpdateDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsActive { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
