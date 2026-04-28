using BooksIO2026.Data.Interfaces;

namespace BooksIO2026.Data
{
    public interface IUnitOfWork
    {
        public IAuthorRepository Authors { get; }
        public IPublisherRepository Publishers { get; }
        public IBookRepository Books { get; }
        void Save();
    }
}
