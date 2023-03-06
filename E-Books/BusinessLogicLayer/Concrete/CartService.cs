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

    public async Task AddItemToCart(Book book , string userId)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task<List<CartBook>> GetShoppingCartItems()
    {
        throw new NotImplementedException();
    }

    public double GetShoppingCartTotal()
    {
        throw new NotImplementedException();
    }

    public Task RemoveItemFromCart(Book book)
    {
        throw new NotImplementedException();
    }
}
