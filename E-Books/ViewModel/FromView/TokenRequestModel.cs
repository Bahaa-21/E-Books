using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;

public class TokenRequestModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
