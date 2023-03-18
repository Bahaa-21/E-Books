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
            cartUser = new Carts(){ UserId = userId};
            await _context.Carts.AddAsync(cartUser);
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


    public double GetCartTotal(int cartId) =>
     _context.CartBooks.Where(c => c.CartId == cartId).Select(c => c.Books.Price * c.Amount).Sum();
    

    public async Task<bool> RemoveItemFromCart(int bookId , int cartId)
    {
        var removeItem = await _context.CartBooks.SingleOrDefaultAsync(c => c.CartId == cartId && c.BookId == bookId);
         _context.CartBooks.Remove(removeItem);
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
