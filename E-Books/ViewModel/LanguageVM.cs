using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.ViewModel
{
    public class LanguageVM
    {
        public int Id { get; set; }
        [Required, MaxLength(35)]
        public string LanguageName { get; set; }
    }
}