using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using X.PagedList;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface IBookService : IDisposable
{
    Task<IPagedList<Book>> GetAllBookAsync(RequestParams requestParams);
    Task<Book> GetBookAsync(int bookId, bool includes);
    Task<IPagedList<Book>> GetBookGenre(int genreId , RequestParams param);
    Task<Book> GetBookWithAuthor(int bookId);

}
