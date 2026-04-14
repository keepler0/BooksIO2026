using BooksIO2026.Data.Interfaces;
using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksIO2026.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BooksDbContext _context;

        public AuthorRepository(BooksDbContext context)
        {
            _context = context;
        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
        }

        public void Delete(int id)
        {
            var authorToDelete = _context.Authors.Find(id);
            if (authorToDelete is null) return;

            _context.Authors.Remove(authorToDelete);
        }

        public bool Exist(string firstName, string lastName, int? id = null)
        {
            Author? author;
            if (id.HasValue)
            {
                author = _context.Authors.FirstOrDefault(a => a.FirstName == firstName &&
                                                             a.LastName == lastName &&
                                                             a.AuthorId != id);
            }
            author = _context.Authors.FirstOrDefault(a => a.FirstName == firstName &&
                                                             a.LastName == lastName);
            return author is not null;
        }

        public List<Author> GetAll()
        {
            return _context.Authors.AsNoTracking()
                                   .ToList();
        }

        public Author? GetById(int id)
        {
            return _context.Authors.Find(id);
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }
    }
}
