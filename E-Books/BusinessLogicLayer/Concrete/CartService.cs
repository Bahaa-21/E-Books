using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer;
using E_Books.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Books.BusinessLogicLayer.Concrete;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddItemToCart(int bookId , int amount = 1 , string userId)
    {
        var addToCart = _context.Carts.Where(u => u.UserId == userId).SingleOrDefault();
        
        if(addToCart is null)
        {
            var cart = new Carts()
            {
                UserId = userId,
            };
            await _context.Carts.AddAsync(cart);
            await  _context.SaveChangesAsync();

            var cartBook = new CartBook(){
                BookId = bookId,
                CartId = cart.Id,
                Amount = amount,
            };
            await _context.CartBooks.AddAsync(cartBook);
            await _context.SaveChangesAsync();
             return true;
        }
        else
        {
            var bookCart = _context.CartBooks.Where(b => b.BookId == bookId && b.CartId == addToCart.Id).SingleOrDefault();
            bookCart.Amount += amount;
             _context.SaveChanges();
            return true;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task<List<CartBook>> GetCartItems(string userId)
    {
       throw new NotImplementedException();
    }

    public double GetCartTotal()
    {
        throw new NotImplementedException();
    }

    public Task RemoveItemFromCart(Book book)
    {
        throw new NotImplementedException();
    }
}
