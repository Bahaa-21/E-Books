using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.Models;
public class Photo
{
    public int Id { get; set; }
    [Required]
    public string? ProfilePhoto {get; set;}
    [Required]
    public DateTime AddedOn {get;set;}
    public string UsersAppId{get; set;}

    [ForeignKey(nameof(UsersAppId))]
    public UsersApp Users {get; set;}
}
