namespace BooksIO2026.Service.DTOs.Book
{
    public class BookListDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string AuthorName { get; set; } = null!;
        public string PublisherName { get; set; }=null!;
        //public int Stock { get; set; }
        //TODO: Agregar el nombre del autor al DTO y el stock del libro
    }
}
