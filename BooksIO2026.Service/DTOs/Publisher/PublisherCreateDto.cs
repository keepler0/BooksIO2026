namespace BooksIO2026.Service.DTOs.Publisher
{
    public class PublisherCreateDto
    {
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateTime FoundedDate { get; set; }
        public string? Email { get; set; }
    }
}
