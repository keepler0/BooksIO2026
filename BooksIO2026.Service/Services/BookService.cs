using BooksIO2026.Data;
using BooksIO2026.Data.Interfaces;
using BooksIO2026.Entities;
using BooksIO2026.Service.DTOs.Book;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Mappers;
using FluentValidation;

namespace BooksIO2026.Service.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<Book> _bookValidator;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IBookRepository bookRepository,
                           IValidator<Book> bookValidator,
                           IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _bookValidator = bookValidator;
            _unitOfWork = unitOfWork;
        }

        public (bool Success, List<string> Errors) Add(BookCreateDto bookDto)
        {
            var book = BookMapper.ToBookEntity(bookDto);
            var result = _bookValidator.Validate(book);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                    return (false, errors);
                }
            }
            if (_bookRepository.Exist(bookDto.Title))
            {
                return (false, new List<string>() { "The book already exist!" });
            }
            try
            {
                _bookRepository.Add(book);
                _unitOfWork.Save();
                return (true, new List<string>());
            }
            catch (Exception)
            {
                return (false, new List<string>() { "Database Error" });
            }
        }
        public (bool Success, List<string> Errors) Delete(int bookId)
        {
            try
            {
                if (_bookRepository.GetById(bookId) is null)
                {
                    return (false, new List<string>() { "Book not found!" });
                }
                _bookRepository.Delete(bookId);
                _unitOfWork.Save();
                return (true, new List<string>());
            }
            catch (Exception)
            {
                return (false, new List<string>() { "Database error" });
            }
        }

        public List<BookListDto> GetAll()
        {
            return _bookRepository.GetAll()
                                  .Select(b => BookMapper.ToBookListDto(b))
                                  .ToList();
        }

        public BookUpdateDto? GetBookForUpdate(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book is not null)
            {
                return BookMapper.ToBookUpdateDto(book);
            }
            return null;
        }

        public BookDetailDto? GetById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book is not null) return BookMapper.ToBookDetailDto(book);
            return null;
        }

        public (bool Success, List<string> Errors) Update(BookUpdateDto bookDto)
        {
            var book = _bookRepository.GetById(bookDto.BookId);
            if (book is null)
            {
                return (false, new List<string>() { "Book not found" });
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
                    var errors = result.Errors
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                    return (false, errors);
                }
            }
            try
            {
                if (!_bookRepository.Exist(book.Title, book.BookId))
                {
                    _unitOfWork.Save();
                    return (true, new List<string>());
                }
                return (false, new List<string>() { "The book already exist!" });
            }
            catch (Exception)
            {
                return (false, new List<string>() { "Database error" });
            }
        }
    }
}
