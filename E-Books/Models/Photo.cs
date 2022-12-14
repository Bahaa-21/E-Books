using System.ComponentModel.DataAnnotations.Schema;

namespace E_Books.Models;
public class Photo
{
    public int Id { get; set; }
    public byte[]? ProfilePhto {get; set;} = null;
    public DateTime AddedOn {get;set;}
    public string UsersAppId{get; set;}

    [ForeignKey(nameof(UsersAppId))]
    public UsersApp Users {get; set;}
}
