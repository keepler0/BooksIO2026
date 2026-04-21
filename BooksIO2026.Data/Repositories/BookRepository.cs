using BooksIO2026.Data.Interfaces;
using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksIO2026.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksDbContext _context;

        public BookRepository(BooksDbContext context)
        {
            _context = context;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public void Delete(int id)
        {
            var bookToDelete = _context.Books.Find(id);
            if (bookToDelete is null) return;
            _context.Books.Remove(bookToDelete);
        }

        public bool Exist(string title, int? id = null)
        {
            Book? book;
            if (id.HasValue)
            {
                book = _context.Books.FirstOrDefault(b => b.Title == title && 
                                                          b.BookId == id.Value);
            }
            book = _context.Books.FirstOrDefault(b => b.Title == title);
            return book is not null;
        }

        public List<Book> GetAll()
        {
            return _context.Books.AsNoTracking()
                                 .ToList();
        }

        public Book? GetById(int id)
        {
            var bookInDb = _context.Books.Include(b => b.Author)
                                         .Include(b => b.Publisher)
                                         .FirstOrDefault(b=>b.BookId==id);
            return bookInDb;
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }
    }
}
