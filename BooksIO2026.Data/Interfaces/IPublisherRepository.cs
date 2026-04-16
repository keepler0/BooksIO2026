using BooksIO2026.Entities;

namespace BooksIO2026.Data.Interfaces
{
    public interface IPublisherRepository
    {
        List<Publisher> GetAll();
        Publisher? GetById(int id);
        void Delete(int id);
        void Update(Publisher publisher);
        void Add(Publisher publisher);
        bool Exist(string? Name=null, string? Country=null, int? id = null);
    }
}
