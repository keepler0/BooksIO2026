using BooksIO2026.Data.Interfaces;
using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksIO2026.Data.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly BooksDbContext _context;

        public PublisherRepository(BooksDbContext context)
        {
            _context = context;
        }

        public void Add(Publisher publisher)
        {
            _context.Add(publisher);
        }

        public void Delete(int id)
        {
            var publisherToDelete = _context.Publishers.Find(id);
            if (publisherToDelete is null) return;
            _context.Publishers.Remove(publisherToDelete);
        }

        public bool Exist(string? Name=null, string? Country=null, int? id = null)
        {
            Publisher? publisher;
            if (id.HasValue && Name is not null && Country is not null)
            {
                publisher = _context.Publishers.AsNoTracking()
                                               .FirstOrDefault(p => p.Name == Name && 
                                                                    p.Country == Country && 
                                                                    p.PublisherId != id.Value);
            }
            if (id.HasValue)
            {
                publisher = _context.Publishers.AsNoTracking()
                                               .FirstOrDefault(p => p.PublisherId == id.Value);
            }
            publisher = _context.Publishers.AsNoTracking()
                                           .FirstOrDefault(p => p.Name == Name &&
                                                                p.Country == Country);
            return publisher is not null;
        }

        public List<Publisher> GetAll()
        {
            return _context.Publishers.AsNoTracking()
                                      .ToList();
        }

        public Publisher? GetById(int id)
        {
            return _context.Publishers.Find(id);
        }

        public bool HasBooks(int id)
        {
            return _context.Books.Any(b => b.PublisherId == id);
        }

        public void Update(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
        }
    }
}
