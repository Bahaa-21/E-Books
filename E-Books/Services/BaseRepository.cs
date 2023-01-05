using System.Linq.Expressions;
using E_Books.Data;
using E_Books.IService;
using E_Books.Models;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace E_Books.Service;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;


    public BaseRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(T entity) => await _context.AddAsync(entity);


    public async Task AddRangeAsync(List<T> entity) => await _context.AddRangeAsync(entity);


    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (expression is not null)
        {
            query = query.Where(expression);
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

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include is not null)
        {
            query = include(query);
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }
    public void Delete(T entity) => _context.Remove(entity);


    public void Update(T entity)
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null,
     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include != null)
            query = include(query);

        if (expression != null)
            query = query.Where(expression);

        if (orderBy != null)
        {
            return await orderBy(query).AsNoTracking().FirstOrDefaultAsync();
        }
        else

            return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public CountsVM GetCount()
    {
        var counts = new CountsVM(){
            CountOfBooks =  _context.Books.Count(),
            CountOfAuthors =  _context.Authors.Count(),
            CountOfPublishers =  _context.Publishers.Count(),
            CountOfUser = _context.Users.Count()
        };

        return counts;
    }

    public async Task<IPagedList<Book>> GetAllBookAsync(RequestParams requestParams)
    {
        return await _context.Books.Include(a => a.Authors)
                             .ThenInclude(a => a.Authors)
                             .Include(l => l.Languages)
                             .Include(p => p.Publishers)
                             .Include(g => g.Genres)
                             .ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    }

    public async Task<Book> GetBookAsync(int id, bool includes)
    {
        if (!includes)
            return await _context.Books.FindAsync(id);

        var books = await _context.Books.Include(a => a.Authors).ThenInclude(a => a.Authors).SingleAsync(bi => bi.Id == id);

        _context.Entry(books).Reference(g => g.Genres).Load();
        _context.Entry(books).Reference(p => p.Publishers).Load();
        _context.Entry(books).Reference(l => l.Languages).Load();

        return books;

    }

    public Task<IEnumerable<Book>> GetBookByGenre(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<T>> Search(RequestParams requestParams = null, Expression<Func<T, bool>> expression = null , Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (include is not null)
        {
            query = include(query);
        }

        if (expression is not null)
        {
            query = query.Where(expression);
        }
        return query.AsNoTracking().ToPagedListAsync(pageNumber: requestParams.PageNumber, pageSize: requestParams.PageSize);
    }
}