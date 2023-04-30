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



    public async Task<Carts> AddItemToCart(string userId, int bookId, int amount = 1)
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
            return cartUser;
        }

        var addAmount = cartUser.CartBooks.Where(b => b.BookId == bookId).SingleOrDefault();
        addAmount.Amount = amount;
        return cartUser;
    }


    public double GetCartTotal(int cartId) =>
     _context.CartBooks.Where(c => c.CartId == cartId).Select(c => c.Books.Price * c.Amount).Sum();
    

    public async Task<CartBook> RemoveItemFromCart(int bookId , int cartId)
    {
        var removeItem = await _context.CartBooks.Include(b => b.Books).SingleOrDefaultAsync(c => c.CartId == cartId && c.BookId == bookId);
         _context.CartBooks.Remove(removeItem);
        return removeItem;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task ClearCartUserItems(int cartId)
    {
        var cartItems =await _context.CartBooks.Where(c => c.CartId == cartId).ToListAsync();
        _context.CartBooks.RemoveRange(cartItems);
    }

    public string AddToCartValidation(int bookId, int bookQty)
    {
        string message = "success";
        var book = _context.Books.Include(c => c.CartBooks).SingleOrDefault(b => b.Id == bookId);
        if(book != null)
        {
            if(book.Quantity == 0) message = "This product is out of stock!";
            else if(book.Quantity < bookQty) message = "The quantity of this product less than your request!";
        }
        else{
            message = "Product is not found";
        }
        return message;
    }
}
