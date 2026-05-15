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
            if (_unitOfWork.Books.ExistSameName(bookDto.Title))
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

        public Result<List<BookListDto>>GetAll()
        {
            var books = _unitOfWork.Books.GetAll()
                                   .Select(b => BookMapper.ToBookListDto(b))
                                   .ToList();
            return Result<List<BookListDto>>.Success(books);
        }

        public Result<BookUpdateDto> GetBookForUpdate(int id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book is null)return Result<BookUpdateDto>.Failure("Book not found");
            return Result<BookUpdateDto>.Success(BookMapper.ToBookUpdateDto(book));
        }

        public Result<List<BookListDto>> GetBooksByPublisher(int id)
        {
            var query = _unitOfWork.Books.Query().Where(b => b.PublisherId == id).Select(b => new BookListDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Price = b.Price,
                IsActive = b.IsActive,
                AuthorName = $"{b.Author!.FirstName} {b.Author!.LastName}",

            }).ToList();
            if (query is null) return Result<List<BookListDto>>.Failure("Data not found");
            return Result<List<BookListDto>>.Success(query);
        }

        public Result<List<BooksGroupedByPublisherDto>> GetBooksGroupedByPublisher()
        {
            var query = _unitOfWork.Books.Query().GroupBy(b => new { b.PublisherId, b.Publisher!.Name })
                .Select(g => new BooksGroupedByPublisherDto
                {
                    PublisherId = g.Key.PublisherId,
                    PublisherName = g.Key.Name,
                    TotalCount = g.Count(),
                    //TotalStock=g.Sum(b=>b.Stock),
                    AveragePrice = g.Average(b => b.Price)
                }).ToList();
            if (query is null) return Result<List<BooksGroupedByPublisherDto>>.Failure("Data not found!");
            return Result<List<BooksGroupedByPublisherDto>>.Success(query);
        }

        public Result<BookDetailDto> GetById(int id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book is null) return Result<BookDetailDto>.Failure("Book not found");
            return Result<BookDetailDto>.Success(BookMapper.ToBookDetailDto(book));
        }

        public Result<BookDetailDto> GetDetail(int id)
        {
            var book= _unitOfWork.Books.GetById(id);
            if (book is null) return Result<BookDetailDto>.Failure("Book not found");
            return Result<BookDetailDto>.Success(BookMapper.ToBookDetailDto(book));
        }

        public Result<List<BookListDto>> GetMoreExpensiveBooks()
        {
            var query= _unitOfWork.Books.Query().OrderByDescending(b=>b.Price).Take(10).Select(b=>new BookListDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Price = b.Price,
                IsActive = b.IsActive,
                AuthorName = $"{b.Author!.FirstName} {b.Author!.LastName}",
                PublisherName = b.Publisher!.Name
            }).ToList();
            if (query is null) return Result<List<BookListDto>>.Failure("Data not found!");
            return Result<List<BookListDto>>.Success(query);
        }

        public Result Update(BookUpdateDto bookDto)
        {
            var book = _unitOfWork.Books.GetById(bookDto.BookId);
            if (book is null)
            {
                return Result.Failure("Book not found");
                //return (false, new List<string>() { "Book not found" });
            }
            var bookToValidate = BookMapper.ToBookEntity(bookDto);
            var result = _bookValidator.Validate(bookToValidate);
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
            book.Title = bookDto.Title;
            book.Price = bookDto.Price;
            book.PublishedDate = bookDto.PublishedDate;
            book.PublisherId = bookDto.PublisherId;
            book.AuthorId = bookDto.AuthorId;
            book.IsActive = bookDto.IsActive;

            if (_unitOfWork.Books.ExistSameName(book.Title, book.BookId))
            {
                return Result.Failure("The book already exist!");
                //return (false, new List<string>() { "The book already exist!" });
            }
            try
            {
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
    }
}
