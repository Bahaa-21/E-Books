using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Books.Models
{
    public class Book_Author
    {
        
        public int Id {get;}

        public int BookId {get; set;}
        public Book Books {get; set;}

         public int AuthorId {get; set;}
        public Author Authors {get; set;}
    }
}