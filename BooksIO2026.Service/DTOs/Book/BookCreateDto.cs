namespace BooksIO2026.Service.DTOs.Book
{
    public class BookCreateDto
    {
        public string Title { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
