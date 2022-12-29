using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;

public class AddRoleModel
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Role { get; set; }
}
