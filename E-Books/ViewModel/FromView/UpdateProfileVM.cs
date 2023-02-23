using System.ComponentModel.DataAnnotations;
using E_Books.Data.Enum;

namespace E_Books.ViewModel.FromView;

public class UpdateProfileVM
{
    public string UserName { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; }
    [StringLength(50)]
    public string LastName { get; set; }
    [StringLength(13)]
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
}