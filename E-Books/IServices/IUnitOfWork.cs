using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.IService;

namespace E_Books.IServices
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository Book { get; } 
        IBaseRepository Author { get; } 
        IBaseRepository BookAuthor { get; } 

        Task SaveAsync();
    }
}