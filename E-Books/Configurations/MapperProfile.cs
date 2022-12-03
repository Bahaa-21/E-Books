using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Books.Models;
using E_Books.ViewModel;

namespace E_Books.Configurations;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region BookMapping
        CreateMap<Book, BookVM>()
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.AuthorId)))
        .ReverseMap()
        .ForMember(d => d.Id, opt => opt.Ignore())
        .ForMember(b => b.Authors, opt => opt.Ignore())
        .AfterMap((bv, b) =>
        {

            //Remeve unselected Authors
            var removeAuthor = b.Authors.Where(b => !bv.Authors.Contains(b.AuthorId));
            foreach (var item in removeAuthor)
                b.Authors.Remove(item);

            //Add new Authors
            var addedAuthors = bv.Authors.Where(id => !b.Authors.Any(a => a.AuthorId == id)).Select(id => new Book_Author { AuthorId = id });
            foreach (var authId in addedAuthors)
                b.Authors.Add(authId);

        });

        CreateMap<Book, ReadBookVM>()
        .ForMember(d => d.PublisherName, opt => opt.MapFrom(sec => sec.Publishers.Name))
        .ForMember(d => d.LanguageName, opt => opt.MapFrom(sec => sec.Languages.LanguageName))
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.AuthorId)))
        .ReverseMap();

        CreateMap<Book, UpdateBookVM>()
        .ReverseMap();

        CreateMap<Book_Author, BooksAuthorsVM>()
        .ReverseMap();

        #endregion

        #region AuthorMapping
        CreateMap<Author, AuthorVM>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(sec => sec.Name))
        .ForMember(dest => dest.Books, opt => opt.MapFrom(sec => sec.Books.Select(sel => sel.Books)))
        .ReverseMap()
        .AfterMap((av, a) =>
        {
            //Remeve unselected Books
            var removeBook = a.Books.Where(a => !av.Books.Contains(a.BookId));
            foreach (var item in removeBook)
                a.Books.Remove(item);

            //Add new Books
            var addedBooks = av.Books.Where(id => !a.Books.Any(a => a.BookId == id)).Select(id => new Book_Author { BookId = id });
            foreach (var bookId in addedBooks)
                a.Books.Add(bookId);

        });

        CreateMap<Author, CreateAuthorVM>()
       .ForMember(dest => dest.FullName, opt => opt.MapFrom(sec => sec.Name))
       .ReverseMap();
        #endregion

        #region PublisherMapping
        CreateMap<Publisher, PublisherVM>().ReverseMap();
        #endregion

        #region LanguageMapping
        CreateMap<BookLanguage, LanguageVM>().ReverseMap();
        #endregion
    }
}