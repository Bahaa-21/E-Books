using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Abstract
{
    public interface IOrdersService
    {
        Task<bool> StoreOrderAsync(int cartId , string userId , string userAddress , string userEmail);
    }
}