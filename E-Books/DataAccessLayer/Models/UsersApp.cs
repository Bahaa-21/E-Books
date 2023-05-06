using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_Books.DataAccessLayer.Enum;
using Microsoft.AspNetCore.Identity;

namespace E_Books.DataAccessLayer.Models;

public class UsersApp : IdentityUser
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }
    [Required, StringLength(50)]
    public string LastName { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required , MaxLength(50)]
    public string Address {get; set;}
    public Photo Photo { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public Carts Carts {get; set;}
    public ICollection<Order> Orders {get; set;}
    public UsersApp()
    {
        Orders = new Collection<Order>();
    }
}