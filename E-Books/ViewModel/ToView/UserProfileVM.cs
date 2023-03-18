using System.ComponentModel.DataAnnotations;

namespace E_Books.ViewModel.ToView
{
    public class UserProfileVM
    {
        public string UserName { get; set; }
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address {get; set;}
        public string ProfilePhoto { get; set; }
        public string Gender { get; set; }
    }
}