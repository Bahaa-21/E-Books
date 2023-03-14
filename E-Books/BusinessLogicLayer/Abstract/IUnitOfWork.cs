using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Book> Book{get;}
        IBaseRepository<Author> Author { get; }
        IBaseRepository<Publisher> Publisher { get; }
        IBaseRepository<BookLanguage> Language { get; }
        IBaseRepository<Genre> Genre { get; }
        IBaseRepository<Photo> Photo { get; }
        IBaseRepository<Book_Author> BookAuthor { get; }
        IBaseRepository<UsersApp> Users { get; }
        IBaseRepository<Carts> Carts {get;}
        IBaseRepository<CartBook> CartBooks {get;}

        Task SaveAsync();
    }
}