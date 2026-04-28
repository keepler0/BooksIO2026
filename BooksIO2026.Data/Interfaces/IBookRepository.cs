using BooksIO2026.Entities;

namespace BooksIO2026.Data.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        Book? GetById(int id);
        void Delete(int id);
        void Update(Book book);
        void Add(Book book);
        bool ExistSameName(string title,int? id = null);
    }
}
