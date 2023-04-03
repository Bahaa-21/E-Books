using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;

namespace E_Books.BusinessLogicLayer.Abstract
{
    public interface IOrdersService
    {
        Task<bool> StoreOrderAsync(int cartId , string userId , string userAddress , string userEmail);
        Task<List<OrderItemsVM>> GetAllOrderDetails(string userId);
    }
}