using BooksIO2026.Data.Interfaces;

namespace BooksIO2026.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BooksDbContext _context;

        public UnitOfWork(BooksDbContext context,
                          IAuthorRepository authors, 
                          IBookRepository books,
                          IPublisherRepository publishers)
        {
            _context = context;
            Authors= authors;
            Publishers= publishers;
            Books= books; 
            
        }

        public IAuthorRepository Authors { get; }

        public IPublisherRepository Publishers { get; }

        public IBookRepository Books { get; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
