using System.Linq.Expressions;
using E_Books.DataAccessLayer;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;
using E_Books.BusinessLogicLayer.Abstract;

namespace E_Books.BusinessLogicLayer.Concrete;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;


    public BaseRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(T entity) => await _context.AddAsync(entity);


    public async Task AddRangeAsync(List<T> entity) => await _context.AddRangeAsync(entity);


    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (include is not null)
        {
            query = include(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }
    public async Task<IPagedList<T>> GetAllAsync(RequestParams requestParams = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include is not null)
        {
            query = include(query);
        }

        return await query.AsNoTracking().ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);

    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include is not null)
        {
            query = include(query);
        }

        return await query.AsNoTracking().FirstAsync(predicate);
    }
    public void Delete(T entity) => _context.Remove(entity);


    public void Update(T entity)
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null,
     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();


        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
            query = include(query);

        if (orderBy != null)
        {
            return await orderBy(query).AsNoTracking().FirstAsync();
        }
        else

            return await query.AsNoTracking().FirstAsync();
    }
    public async Task<T> Include(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        return await query.AsNoTracking().SingleAsync();
    }

    public CountsVM GetCount()
    {
        var counts = new CountsVM()
        {
            CountOfBooks = _context.Books.Count(),
            CountOfAuthors = _context.Authors.Count(),
            CountOfPublishers = _context.Publishers.Count(),
            CountOfUser = _context.Users.Count()
        };

        return counts;
    }

    public Task<IPagedList<T>> Search(RequestParams requestParams = null, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        return query.AsNoTracking().ToPagedListAsync(pageNumber: requestParams.PageNumber, pageSize: requestParams.PageSize);
    }
}