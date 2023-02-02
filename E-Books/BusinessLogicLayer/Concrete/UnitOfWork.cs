using E_Books.BusinessLogicLayer.Abstract;
using E_Books.Data;
using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Concrete
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
        private IBaseRepository<Publisher> _publishers;
        private IBaseRepository<BookLanguage> _languages;
        private IBaseRepository<Genre> _genres;
        private IBaseRepository<Photo> _photo;
        private IBaseRepository<Book_Author> _booksauthors;
        private IBaseRepository<UsersApp> _users;

        public IBaseRepository<Book> Book => _books ??= new BaseRepository<Book>(_context);

        public IBaseRepository<Author> Author => _authors ??= new BaseRepository<Author>(_context);

        public IBaseRepository<Book_Author> BookAuthor => _booksauthors ??= new BaseRepository<Book_Author>(_context);

        public IBaseRepository<Publisher> Publisher => _publishers ??= new BaseRepository<Publisher>(_context);

        public IBaseRepository<BookLanguage> Language => _languages ??= new BaseRepository<BookLanguage>(_context);

        public IBaseRepository<Genre> Genre => _genres ??= new BaseRepository<Genre>(_context);
        public IBaseRepository<Photo> Photo => _photo ??= new BaseRepository<Photo>(_context);
        public IBaseRepository<UsersApp> Users => _users ??= new BaseRepository<UsersApp>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}