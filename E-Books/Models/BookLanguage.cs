using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.Models;

[Table(("BooksLanguages"))]
public class BookLanguage
{
    public int Id { get; set; }
    [Required , MaxLength(35)]
    public string LanguageName { get; set; }
    public List<Book> Books {get; set;}
    public BookLanguage()
    {
        Books = new List<Book>();
    }

}