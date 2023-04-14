using System.ComponentModel.DataAnnotations;
namespace E_Books.ViewModel.ToView;

public class RolesVM
{
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string NormalizedName { get; set; }
}
