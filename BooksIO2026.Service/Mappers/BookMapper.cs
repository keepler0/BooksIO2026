using BooksIO2026.Entities;
using BooksIO2026.Service.DTOs.Book;

namespace BooksIO2026.Service.Mappers
{
    public static class BookMapper
    {
        public static BookListDto ToBookListDto(Book book)
        {
            return new BookListDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Price = book.Price,
                IsActive = book.IsActive
            };
        }
        public static BookUpdateDto ToBookUpdateDto(Book book)
        {
            return new BookUpdateDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Price = book.Price,
                PublishedDate = book.PublishedDate,
                AuthorId = book.AuthorId,
                PublisherId = book.PublisherId,
                IsActive = book.IsActive,
            };
        }
        public static Book ToBookEntity(BookUpdateDto bookDto)
        {
            return new Book
            {
                BookId = bookDto.BookId,
                Title = bookDto.Title,
                Price = bookDto.Price,
                PublishedDate = bookDto.PublishedDate,
                AuthorId = bookDto.AuthorId,
                PublisherId = bookDto.PublisherId,
                IsActive = bookDto.IsActive,
            };
        }
        public static BookDetailDto ToBookDetailDto(Book book)
        {
            return new BookDetailDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Price = book.Price,
                PublishedDate = book.PublishedDate,
                IsActive = book.IsActive,
                AuthorName = $"{book.Author!.FirstName} {book.Author.LastName}",
                PublisherName = $"{book.Publisher!.Name}"
            };
        }
        public static Book ToBookEntity(BookCreateDto bookDto)
        {
            return new Book
            {
                Title = bookDto.Title,
                Price = bookDto.Price,
                PublishedDate = bookDto.PublishedDate,
                PublisherId = bookDto.PublisherId,
                AuthorId = bookDto.AuthorId,
                IsActive = bookDto.IsActive,
            };
        }
    }
}
