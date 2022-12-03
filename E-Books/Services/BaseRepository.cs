using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E_Books.Data;
using E_Books.IService;
using E_Books.Models;
using E_Books.ViewModel;
using Microsoft.EntityFrameworkCore;

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

    public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression = null, string[] includes = null) where T : class
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

    public async Task<IList<BookVM>> GetAllBookAsync() => await _context.Books
    .Select(sec => new BookVM()
    {
        Title = sec.Title,
        Description = sec.Description,
        Price = sec.Price,
        NumberPages = sec.NumberPages,
      
        PublicationDate = sec.PublicationDate,
        
    }).AsNoTracking().ToListAsync();

    public async Task<BookVM> GetBookAsync(int id ) => await _context.Books.Where(i => i.Id == id)
    .Select(book => new BookVM()
    {
        Title = book.Title,
        Description = book.Description,
        Price = book.Price,
        NumberPages = book.NumberPages,
        
        PublicationDate = book.PublicationDate,
        
    }).FirstOrDefaultAsync();

}