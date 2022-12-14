using E_Books.Data;
using E_Books.IService;
using E_Books.IServices;
using E_Books.Models;
using E_Books.Service;

namespace E_Books.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IBaseRepository<Book> _books;
        private IBaseRepository<Author> _authors;
        private IBaseRepository<Book_Author> _booksauthors;
        
        public IBaseRepository<Book> Book => _books??= new BaseRepository<Book>(_context);

        public IBaseRepository<Author> Author => _authors??= new BaseRepository<Author>(_context);

        public IBaseRepository<Book_Author> BookAuthor => _booksauthors??= new BaseRepository<Book_Author>(_context);

        

        public void Dispose()
        {
           _context.Dispose();
           GC.SuppressFinalize(this);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}