using E_Books.IService;
using E_Books.Models;
using E_Books.Service;

namespace E_Books.IServices
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Book> Book { get; } 
        IBaseRepository<Author> Author { get; } 
        IBaseRepository<Book_Author> BookAuthor { get; } 

        Task SaveAsync();
    }
}