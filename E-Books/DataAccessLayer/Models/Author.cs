using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.DataAccessLayer.Models;

public class Author
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<Book_Author> Books { get; set; }
    public Author()
    {
        Books = new Collection<Book_Author>();
    }

}