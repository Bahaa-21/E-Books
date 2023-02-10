using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;

public class UpdateProfileVM
{
    
    [StringLength(50)]
    public string FirstName { get; set; }
    [StringLength(50)]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
}