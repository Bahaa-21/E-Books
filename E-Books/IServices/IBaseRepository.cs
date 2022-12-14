using System.Linq.Expressions;
using E_Books.Models;
using E_Books.ViewModel;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace E_Books.IService;

public interface IBaseRepository<T> where T : class
{
    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<IPagedList<T>> GetAllAsync(RequestParams requestParams = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<T> GetAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<T> GetAsync(Expression<Func<T, bool>> expression = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);


    Task AddAsync(T entity);
    Task AddRangeAsync(List<T> entity);

    void Update(T entity);
    void Delete(T entity);

    Task<Book> GetBookAsync(int id, bool includes);

    Task<IEnumerable<Book>> GetAllBookAsync();
    Task<IEnumerable<Book>> GetBookByGenre(int id);
    Task<IPagedList<Book>> GetAllBookAsync(RequestParams requestParams);
}
