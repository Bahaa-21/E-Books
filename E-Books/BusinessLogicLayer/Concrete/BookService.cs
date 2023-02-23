using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.Data;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace E_Books.BusinessLogicLayer.Concrete
{
    public class BookService : IBookService 
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context) 
        {
            _context = context ;
        }

        public void Dispose()
        {
           _context.Dispose();
            GC.SuppressFinalize(this);
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

        public async Task<Book> GetBookAsync(int bookId, bool includes)
        {
            if (!includes)
                return await _context.Books.FindAsync(bookId);

            var book = await _context.Books.Include(a => a.Authors).ThenInclude(a => a.Authors).SingleAsync(bi => bi.Id == bookId);
            _context.Entry(book).Reference(g => g.Genres).Load();
            _context.Entry(book).Reference(p => p.Publishers).Load();
            _context.Entry(book).Reference(l => l.Languages).Load();

            return book;

        }

        public async Task<IEnumerable<Book>> GetBookGenre(int genreId) => await _context.Books.Where(genre => genre.GenreId == genreId)
                                            .Include(a => a.Authors).ThenInclude(a => a.Authors)
                                            .Include(genre => genre.Genres)
                                            .Include(publisher => publisher.Publishers)
                                            .Include(language => language.Languages)
                                            .ToListAsync();

        public async Task<Book> GetBookWithAuthor(int bookId) => await _context.Books.Include(book => book.Authors).ThenInclude(bookAuthor => bookAuthor.Authors).SingleAsync(book => book.Id == bookId);
    }
}