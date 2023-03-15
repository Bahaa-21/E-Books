using AutoMapper;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.EntityFrameworkCore;
using E_Books.DataAccessLayer.Models;

namespace E_Books.Configurations;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region Publisher Map
        CreateMap<Publisher, PublisherVM>();
        #endregion


        #region Genre Map
        CreateMap<Genre, GenreVM>().ReverseMap();
        #endregion


        #region Author Map
        CreateMap<Author, AuthorVM>()
        .ForMember(d => d.AuthorName , act => act.MapFrom(sec => sec.Name))
        .ReverseMap();

        CreateMap<Author, ReadAuthorVM>()
        .ForMember(d => d.AuthorName , act => act.MapFrom(sec => sec.Name))
        .ReverseMap();


        CreateMap<Author, BooksAuthorVM>()
        .ForMember(d => d.BookTitle, opt => opt.MapFrom(sec => sec.Books))
        .ForMember(d => d.BookTitle, opt => opt.MapFrom(sec => sec.Books.Select(b => new
        {
            Id = b.Books.Id,
            Title = b.Books.Title
        }).ToList()))
        .ReverseMap();

        #endregion


        #region Language Map
        CreateMap<BookLanguage, LanguageVM>().ReverseMap();
        #endregion


        #region Book Map
        CreateMap<Book, BookVM>()
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.AuthorId)))
        .ReverseMap()
        .ForMember(b => b.Authors, opt => opt.Ignore ())
        .AfterMap( (bookVm, book) =>
        {
            //Remeve unselected Authors
            var removeAuthor = book.Authors.Where(book => !bookVm.Authors.Contains(book.AuthorId));
            foreach (var item in removeAuthor.ToList())
                book.Authors.Remove(item);
            

            //Add new Authors
            var addedAuthors = bookVm.Authors.Where(id => !book.Authors.Any(a => a.AuthorId == id)).Select(id => new Book_Author
            {
                AuthorId = id
            });

            foreach (var authId in addedAuthors)
                book.Authors.Add(authId);

        });

        CreateMap<Book, ReadBookVM>()
        .ForMember(d => d.Publishers, opt => opt.MapFrom(sec => sec.Publishers.Name))
        .ForMember(d => d.Language , opt => opt.MapFrom(sec => sec.Languages.LanguageName))
        .ForMember(d => d.GenreType , opt => opt.MapFrom(sec => sec.Genres.Name))
        .ForMember(d => d.Authors , opt => opt.MapFrom(sec => sec.Authors))
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(author => author.Authors.Name).ToList()))
        .ReverseMap();
       
        CreateMap<Book, SearchBookVM>()
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.Authors.Name).ToList()))
        .ReverseMap();
        #endregion 


        #region Admin Map
        CreateMap<UsersApp , UserProfileVM>()
        .ForMember(des => des.ProfilePhoto , opt => opt.MapFrom(sec => sec.Photos.Image))
        .ReverseMap();
        
        CreateMap<UsersApp , UpdateProfileVM>().ReverseMap();
        #endregion


        #region Cart Map
        CreateMap<CartBook , CartsVM>()
        .ForMember(d => d.BookId , opt => opt.MapFrom(sec => sec.Books.Id))
        .ForMember(d => d.BookName , opt => opt.MapFrom(sec => sec.Books.Title))
        .ForMember(d => d.Price , opt => opt.MapFrom(sec => sec.Books.Price))
        .ForMember(d => d.Amount , opt => opt.MapFrom(sec => sec.Amount))
        .ForMember(d => d.AddedOn , opt => opt.MapFrom(sec => sec.AddedOn));
        #endregion
    }
}