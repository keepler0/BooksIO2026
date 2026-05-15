namespace BooksIO2026.Service.DTOs.Book
{
    public class BooksGroupedByPublisherDto
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = null!;
        public int TotalCount { get; set; }
        public int TotalStock { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
