namespace BooksIO2026.Service.DTOs.Book
{
    public class BookListDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
