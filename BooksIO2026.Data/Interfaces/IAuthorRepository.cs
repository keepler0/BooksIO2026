using BooksIO2026.Entities;

namespace BooksIO2026.Data.Interfaces
{
    public interface IAuthorRepository
    {
        List<Author> GetAll();
        Author? GetById(int id);
        void Delete(int id);
        void Update(Author author);
        void Add(Author author);
        bool ExistSameName(string FirstName, string LastName, int? id=null);
        bool HasBooks(int id);
    }
}
