using BooksIO2026.Service.DTOs;
using BooksIO2026.Service.DTOs.Book;

namespace BooksIO2026.Service.Interfaces
{
    public interface IBookService
    {
        List<BookListDto> GetAll();
        BookDetailDto? GetById(int id);
        (bool Success, List<string> Errors) Add(BookCreateDto bookDto);
        (bool Success, List<string> Errors) Update(BookUpdateDto bookDto);
        (bool Success, List<string> Errors) Delete(int bookId);
        BookUpdateDto? GetBookForUpdate(int id);
    }
}
