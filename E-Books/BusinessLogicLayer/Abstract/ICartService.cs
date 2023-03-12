using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface ICartService : IDisposable
{
    Task<List<CartBook>> GetCartItems(string userId);
    double GetCartTotal();
    Task<bool> AddItemToCart(int bookId , int amount = 1 , string userId);
    Task RemoveItemFromCart(Book book);
}