﻿using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Identity;

namespace E_Books.BusinessLogicLayer.Abstract;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterModel model);
    Task<AuthModel> GetTokenAsync(TokenRequestModel model);
    Task<UsersApp> CreateAdminAccountAsync(CreateAdminVM model);
    Task<string> CreateAdminAccountValidate(string role, string email);
    Task<string> ConfirmEmailAsync(string userId, string code);
    Task<string> DeleteAdminAsync(string adminId);
    Task<string> UpdateRoleAsync(IdentityRole role);
    Task<IList<UsersApp>> GetAllAdmins();
    Task<List<IdentityRole>> GetAllRolesAsync();
    Task<IdentityRole> GetRoleAsync(string roleId);
    Task<AuthModel> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}
