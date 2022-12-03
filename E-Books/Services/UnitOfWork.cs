using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.Data;
using E_Books.IService;
using E_Books.IServices;
using E_Books.Service;

namespace E_Books.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IBaseRepository _books;
        private IBaseRepository _authors;
        private IBaseRepository _booksauthors;
        
        public IBaseRepository Book => _books??= new BaseRepository(_context);

        public IBaseRepository Author => _authors??= new BaseRepository(_context);

        public IBaseRepository BookAuthor => _booksauthors??= new BaseRepository(_context);

        public void Dispose()
        {
           _context.Dispose();
           GC.SuppressFinalize(this);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}