using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;

public class KeyResource
{
    public int Id {get;set;}
    [Required]
    public string Name {get; set;}
}
