using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface ICartService :  IDisposable
{
    double GetCartTotal(int cartId);
    Task<Carts> AddItemToCart(string userId , int bookId , int amount = 1);
    Task<bool> RemoveItemFromCart(int cart , int book);
    string AddToCartValidation(int bookId , int bookQty);
    Task ClearCartUserItems(int cart);
}