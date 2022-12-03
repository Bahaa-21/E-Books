using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.ViewModel
{
    public class PublisherVM
    {
        public int Id {get; set;}
        
        [Required , MaxLength(50)]
        public string Name {get; set;}
    }
}