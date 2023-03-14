using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.DataAccessLayer.Models;

public class Carts
{
    public int Id { get; set; }
    public string UserId {get; set;}
    [ForeignKey("UserId")]
    public UsersApp Users {get; set;}
    public ICollection<CartBook> CartBooks { get; set; }
    public Carts()
    {
        CartBooks = new Collection<CartBook>();
    }
}