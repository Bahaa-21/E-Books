using E_Books.DataAccessLayer.Models;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface IUserService
{
    Task<UsersApp> GetUserProfile();
    Task<bool> UpdateProfile(UsersApp user);
}
