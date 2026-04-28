using BooksIO2026.Data;
using BooksIO2026.Entities;
using BooksIO2026.Service.Common;
using BooksIO2026.Service.DTOs.Book;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Mappers;
using FluentValidation;

namespace BooksIO2026.Service.Services
{
    public class BookService : IBookService
    {
        //private readonly IBookRepository _bookRepository;
        private readonly IValidator<Book> _bookValidator;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(//IBookRepository bookRepository,
                           IValidator<Book> bookValidator,
                           IUnitOfWork unitOfWork)
        {
            //_bookRepository = bookRepository;
            _bookValidator = bookValidator;
            _unitOfWork = unitOfWork;
        }

        public Result Add(BookCreateDto bookDto)
        {
            var book = BookMapper.ToBookEntity(bookDto);
            var result = _bookValidator.Validate(book);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return Result.Failure(result.Errors.Select(e => e.ErrorMessage).ToList());
                    //var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                    //return (false, errors);
                }
            }
            if (_unitOfWork.Books.Exist(bookDto.Title))
            {
                return Result.Failure("The book already exist!");
                //return (false, new List<string>() { "The book already exist!" });
            }
            try
            {
                _unitOfWork.Books.Add(book);
                _unitOfWork.Save();
                return Result.Success();
                //return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
                //return (false, new List<string>() { "Database Error" });
            }
        }
        public Result Delete(int bookId)
        {
            try
            {
                if (_unitOfWork.Books.GetById(bookId) is null)
                {
                    return Result.Failure("Book not found!");
                    // (false, new List<string>() { "Book not found!" });
                }
                _unitOfWork.Books.Delete(bookId);
                _unitOfWork.Save();
                return Result.Success();
                //return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
                //return (false, new List<string>() { "Database error" });
            }
        }

        public List<BookListDto> GetAll()
        {
            return _unitOfWork.Books.GetAll()
                                    .Select(b => BookMapper.ToBookListDto(b))
                                    .ToList();
        }

        public BookUpdateDto? GetBookForUpdate(int id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book is not null)
            {
                return BookMapper.ToBookUpdateDto(book);
            }
            return null;
        }

        public BookDetailDto? GetById(int id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book is not null) return BookMapper.ToBookDetailDto(book);
            return null;
        }

        public Result Update(BookUpdateDto bookDto)
        {
            var book = _unitOfWork.Books.GetById(bookDto.BookId);
            if (book is null)
            {
                return Result.Failure("Book not found");
                //return (false, new List<string>() { "Book not found" });
            }

            book.Title = bookDto.Title;
            book.Price = bookDto.Price;
            book.PublishedDate = bookDto.PublishedDate;
            book.PublisherId = bookDto.PublisherId;
            book.AuthorId = bookDto.AuthorId;
            book.IsActive = bookDto.IsActive;

            var result = _bookValidator.Validate(book);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return Result.Failure(result.Errors
                                                .Select(e => e.ErrorMessage)
                                                .ToList());
                    //var errors = result.Errors
                    //                   .Select(e => e.ErrorMessage)
                    //                   .ToList();
                    //return (false, errors);
                }
            }
            try
            {
                if (!_unitOfWork.Books.Exist(book.Title, book.BookId))
                {
                    _unitOfWork.Save();
                    return Result.Success();
                    //return (true, new List<string>());
                }
                return Result.Failure("The book already exist!");
                //return (false, new List<string>() { "The book already exist!" });
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
                //return (false, new List<string>() { "Database error" });
            }
        }
    }
}
