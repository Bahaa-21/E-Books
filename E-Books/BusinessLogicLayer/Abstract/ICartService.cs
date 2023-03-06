using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface ICartService : IDisposable
{
    Task<List<CartBook>> GetShoppingCartItems();
    double GetShoppingCartTotal();
    Task AddItemToCart(Book book , string userId);
    Task RemoveItemFromCart(Book book);
}