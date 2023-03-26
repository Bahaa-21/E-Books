using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.DataAccessLayer.Models;
public class Photo
{
    public int Id { get; set; }
    public string Image {get; set;}
    public DateTime CreatedOn {get; set;}
    public string UserId {get; set;}
    [ForeignKey("UserId")]
    public UsersApp User {get; set;}

    public Photo()
    {
        CreatedOn = DateTime.UtcNow;
    }
}