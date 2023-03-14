using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.EntityFrameworkCore;

namespace E_Books.BusinessLogicLayer.Concrete;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    public CartService(ApplicationDbContext context) => _context = context;



    public async Task<bool> AddItemToCart(string userId, int bookId, int amount = 1)
    {
        var cartUser = _context.Carts.Where(c => c.UserId == userId).Include(cb => cb.CartBooks).SingleOrDefault();

        if (cartUser is null)
        {
            cartUser = new Carts()
            {
                UserId = userId
            };

            var cartBook = new CartBook() { BookId = bookId, Amount = amount };
            cartUser.CartBooks.Add(cartBook);
            await _context.Carts.AddAsync(cartUser);
            return true;
        }
        if (!cartUser.CartBooks.Any(b => b.BookId == bookId))
        {
            var cartBook = new CartBook() { BookId = bookId, Amount = amount };
            cartUser.CartBooks.Add(cartBook);
            return true;
        }

        var addAmount = cartUser.CartBooks.Where(b => b.BookId == bookId).SingleOrDefault();
        addAmount.Amount = amount;
        return true;
    }


    public async Task<List<CartsVM>> GetCartItems(int cartId)
    {
        var cart = await _context.CartBooks.Where(c => c.CartId == cartId).Include(b => b.Books).Select(sec => new CartsVM()
        {
            BookName = sec.Books.Title,
            Price = sec.Books.Price,
            Amount = sec.Amount,
            AddedOn = sec.AddedOn,
        }).ToListAsync();

        return cart;
    }



    public double GetCartTotal()
    {
        throw new NotImplementedException();
    }

    public Task RemoveItemFromCart(Book book)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
