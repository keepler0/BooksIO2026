using BooksIO2026.Service.DTOs.Book;

namespace BooksIO2026.Service.DTOs
{
    public class AuthorDetailDto
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public List<BookListDto> books { get; set; } = [];
    }
}
