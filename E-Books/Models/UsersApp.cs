using System.ComponentModel.DataAnnotations;
using E_Books.Data.Enum;
using Microsoft.AspNetCore.Identity;

namespace E_Books.Models;

public class UsersApp : IdentityUser
{
    [Required , StringLength(50)]
    public string FirstName {get; set;}
    [Required , StringLength(50)]
    public string LastName {get; set;}
    [Required]
    public Gender Gender{get; set;} 
    public List<RefreshToken> RefreshTokens { get; set;}
}