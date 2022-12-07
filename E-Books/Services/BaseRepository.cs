using System.Linq.Expressions;
using E_Books.Data;
using E_Books.IService;
using E_Books.Models;
using E_Books.ViewModel;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace E_Books.Service;

public class BaseRepository : IBaseRepository
{
    private readonly ApplicationDbContext _context;


    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync<T>(T entity) where T : class
    {
        await _context.AddAsync(entity);
    }


    public async Task AddRangeAsync<T>(List<T> entity) where T : class => await _context.AddRangeAsync(entity);


    public async Task<IList<T>> GetAllAsync<T>(Expression<Func<T, bool>> expression = null, string[] includes = null) where T : class
    {
        IQueryable<T> query = _context.Set<T>();
        if (expression is not null)
        {
            query = query.Where(expression);
        }
        if (includes is not null)
        {
            foreach (var item in includes)
                query = query.Include(item);
        }
        return await query.AsNoTracking().ToListAsync();
    }
    public async Task<X.PagedList.IPagedList<T>> GetAllAsync<T>(RequestParams requestParams = null, string[] includes = null) where T : class
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
        {
            foreach (var item in includes)
                query = query.Include(item);
        }

        return await query.AsNoTracking().ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);

    }

    public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression = null, string[] includes = null ) where T : class
    {
        IQueryable<T> query = _context.Set<T>();
        if (includes is not null)
        {
            foreach (var item in includes)
                query = query.Include(item);
        }
        
        return await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }
    public void Delete<T>(T entity) where T : class => _context.Remove(entity);

    
    public void Update<T>(T entity) where T : class
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

    }

    public async Task<Book> GetBookAsync(int id, bool includes)
    {
        if (!includes)
            return await _context.Books.FindAsync(id);

        return await _context.Books.Include(a => a.Authors)
                                    .ThenInclude(a => a.Authors)
                                    .Include(l => l.Languages)
                                    .Include(p => p.Publishers)
                                    .Include(g => g.Genres)
                                    .SingleOrDefaultAsync(bi => bi.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllBookAsync() =>

        await _context.Books.Include(a => a.Authors)
                            .ThenInclude(a => a.Authors)
                            .Include(l => l.Languages)
                            .Include(p => p.Publishers)
                            .Include(g => g.Genres)
                            .ToListAsync();


    public async Task<IPagedList<Book>> GetAllBookAsync(RequestParams requestParams)
    {
           return await _context.Books.Include(a => a.Authors)
                                .ThenInclude(a => a.Authors)
                                .Include(l => l.Languages)
                                .Include(p => p.Publishers)
                                .Include(g => g.Genres)
                                .ToPagedListAsync(requestParams.PageNumber , requestParams.PageSize);
    }
}