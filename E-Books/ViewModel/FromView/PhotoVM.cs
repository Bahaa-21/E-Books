using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.FromView;
public class PhotoVM
{
   [Required]
   public string ProfilePhoto {get; set;}
}