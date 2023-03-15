using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface ICartService :  IDisposable
{
    double GetCartTotal(int cartId);
    Task<bool> AddItemToCart(string userId , int bookId , int amount = 1);
    Task<bool> RemoveItemFromCart(int cart , int book);
}