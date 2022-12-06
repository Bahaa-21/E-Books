using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.ViewModel;

public class AuthorVM
{
    public int Id { get; set; }

    [Required, StringLength(40)]

    public string Name { get; set; }

}