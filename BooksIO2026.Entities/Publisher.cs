namespace BooksIO2026.Entities
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateTime FoundedDate { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Book> Books { get; set; }=new List<Book>();
    }
}
