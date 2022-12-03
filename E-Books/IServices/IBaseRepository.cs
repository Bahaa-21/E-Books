using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E_Books.Models;
using E_Books.ViewModel;

namespace E_Books.IService;

public interface IBaseRepository
{
    //Genegric Method
    Task<IList<T>> GetAllAsync<T>(Expression<Func<T, bool>> expression = null ,string[] includes = null) where T : class;
    Task<T> GetAsync<T>(Expression<Func<T, bool>> expression = null , string[] includes = null) where T : class;
  
    Task AddAsync<T>(T entity) where T : class;
    Task AddRangeAsync<T>(List<T> entity) where T : class;

    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    
    Task<IList<BookVM>> GetAllBookAsync();
    Task<BookVM> GetBookAsync(int id);

}
