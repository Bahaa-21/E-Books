using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using E_Books.Models;

namespace E_Books.ViewModel;
public class BookVM
{
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Title { get; set; }
    [Required, StringLength(150)]
    public string Description { get; set; }
    [Required]
    public int NumberPages { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public DateTime PublicationDate { get; set; }
    [Required]
    public int PublisherId { get; set; }
    [Required]
    public int LanguagesId { get; set; }
    [Required]
    public ICollection<int> Authors { get; set; }

}
public class ReadBookVM
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int NumberPages { get; set; }

    public double Price { get; set; }

    public DateTime PublicationDate { get; set; }

    public string PublisherName {get; set;}
    public string LanguageName {get; set;}
    
    public ICollection<int> Authors { get; set; }
}

public class BooksAuthorsVM
{
    public int BookId { get; set; }
    public BookVM Books { get; set; }
    public int AuthorId { get; set; }
    public AuthorVM Authors { get; set; }
}

public class UpdateBookVM : BookVM
{
    
}