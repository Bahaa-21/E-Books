using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;
public class PhotoVM
{
   [Required]
   public IFormFile Image {get; set;}
}