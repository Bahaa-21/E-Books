using System.Linq.Expressions;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;
using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface IBaseRepository<T> where T : class
{
    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate  = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<IPagedList<T>> GetAllAsync(RequestParams requestParams = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<T> GetAsync(Expression<Func<T, bool>> predicate  = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<T> GetAsync(Expression<Func<T, bool>> predicate  = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<T> Include(Expression<Func<T, bool>> predicate  = null , Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null);
    Task AddAsync(T entity);
    Task AddRangeAsync(List<T> entity);

    void Update(T entity);
    void Delete(T entity);
    Task<IPagedList<T>> Search(RequestParams requestParams = null, Expression<Func<T, bool>> predicate  = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    CountsVM GetCount();
}
