using System.ComponentModel.DataAnnotations;
using E_Books.DataAccessLayer.Enum;

namespace E_Books.ViewModel.FromView;

public class UpdateProfileVM
{
    [Required,StringLength(50)]
    public string FirstName { get; set; }
    [Required,StringLength(50)]
    public string LastName { get; set; }
    [Required,StringLength(13)]
    public string PhoneNumber { get; set; }
    [Required]
    public string Address {get; set;}
    [Required]
    public Gender Gender { get; set; }
}