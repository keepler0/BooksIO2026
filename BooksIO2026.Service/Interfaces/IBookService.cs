using BooksIO2026.Service.Common;
using BooksIO2026.Service.DTOs.Book;

namespace BooksIO2026.Service.Interfaces
{
    public interface IBookService
    {
        Result<List<BookListDto>> GetAll();
        Result<BookUpdateDto> GetBookForUpdate(int id);
        Result<BookDetailDto> GetById(int id);
        Result<BookDetailDto> GetDetail(int id);
        Result Add(BookCreateDto bookDto);
        Result Update(BookUpdateDto bookDto);
        Result Delete(int bookId);
        //List<BookListDto> GetAll();
        //BookUpdateDto? GetBookForUpdate(int id);
        //BookDetailDto? GetById(int id);
        //(bool Success, List<string> Errors) Add(BookCreateDto bookDto);
        //(bool Success, List<string> Errors) Update(BookUpdateDto bookDto);
        //(bool Success, List<string> Errors) Delete(int bookId);
    }
}
