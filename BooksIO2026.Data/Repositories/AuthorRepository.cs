using BooksIO2026.Data.Interfaces;
using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksIO2026.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        public void Add(Author author)
        {
            using (var context = new BooksDbContext())
            {
                context.Authors.Add(author);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new BooksDbContext())
            {
                var authorToDelete = GetById(id);
                if (authorToDelete is null) return;

                context.Authors.Remove(authorToDelete);
                context.SaveChanges();
            }
        }

        public bool Exist(string firstName, string lastName,int? id=null)
        {
            using (var context=new BooksDbContext())
            {
                Author? author;
                if (id.HasValue)
                {
                    author = context.Authors.FirstOrDefault(a => a.FirstName == firstName &&
                                                                 a.LastName == lastName &&
                                                                 a.AuthorId!=id);
                }
                author = context.Authors.FirstOrDefault(a => a.FirstName == firstName &&
                                                                 a.LastName == lastName);
                return author is not null;
            }
        }

        public List<Author> GetAll()
        {
            using (var context = new BooksDbContext())
            {
                return context.Authors
                              .AsNoTracking()
                              .ToList();
            }
        }

        public Author? GetById(int id)
        {
            using (var context = new BooksDbContext())
            {
                return context.Authors.Find(id);
            }
        }

        public void Update(Author author)
        {
            using (var context = new BooksDbContext())
            {
                context.Authors.Update(author);
                context.SaveChanges();
            }
        }
    }
}
