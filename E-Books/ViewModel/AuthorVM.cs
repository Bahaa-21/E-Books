using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.ViewModel
{
    public class AuthorVM : CreateAuthorVM
    {
        public ICollection<int> Books {get; set;}
    }

    public class CreateAuthorVM
    {
         public int Id {get; set;}
        [Required , StringLength(40)]
        public string FullName {get; set;}

    }
}