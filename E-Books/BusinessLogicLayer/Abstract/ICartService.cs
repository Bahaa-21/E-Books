using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface ICartService :  IDisposable
{
    Task<List<CartsVM>> GetCartItems(int cartId);
    double GetCartTotal();
    Task<bool> AddItemToCart(string userId , int bookId , int amount = 1);
    Task RemoveItemFromCart(Book book);
}