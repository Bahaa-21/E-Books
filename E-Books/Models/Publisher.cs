using System.ComponentModel.DataAnnotations;

namespace E_Books.Models;

public class Publisher
{
    public int Id { get; set; }
    [Required , MaxLength(50)]
    public string Name {get; set;}

    public List<Book> Books {get; set;}
}