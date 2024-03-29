using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer;
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

            var book = await _context.Books.Include(a => a.Authors).ThenInclude(a => a.Authors).SingleOrDefaultAsync(bi => bi.Id == bookId);
            if (book is not null)
            {
                _context.Entry(book).Reference(g => g.Genres).Load();
                _context.Entry(book).Reference(p => p.Publishers).Load();
                _context.Entry(book).Reference(l => l.Languages).Load();

                return book;
            }
            return null;
        }

        public async Task<IPagedList<Book>> GetBookGenre(int genreId , RequestParams param) => await _context.Books.Where(genre => genre.GenreId == genreId)
                                            .Include(a => a.Authors).ThenInclude(a => a.Authors)
                                            .Include(genre => genre.Genres)
                                            .Include(publisher => publisher.Publishers)
                                            .Include(language => language.Languages)
                                            .ToPagedListAsync(pageNumber : param.PageNumber , pageSize:param.PageSize);

        public async Task<Book> GetBookWithAuthor(int bookId) => await _context.Books.Include(book => book.Authors).ThenInclude(bookAuthor => bookAuthor.Authors).SingleOrDefaultAsync(book => book.Id == bookId);
    }
}