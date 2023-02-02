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
        .AfterMap( (bv, b) =>
        {
            //Remeve unselected Authors
            var removeAuthor = b.Authors.Where(b => !bv.Authors.Contains(b.AuthorId));
            foreach (var item in removeAuthor.ToList())
                b.Authors.Remove(item);
            

            //Add new Authors
            var addedAuthors = bv.Authors.Where(id => !b.Authors.Any(a => a.AuthorId == id)).Select(id => new Book_Author
            {
                AuthorId = id
            });

            foreach (var authId in addedAuthors)
                b.Authors.Add(authId);

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
        CreateMap<UsersApp , AdminProfileVM>()
        .ForMember(des => des.Gender , opt => opt.MapFrom(sec => sec.Gender))
        .ForMember(des => des.ProfilePhoto , opt => opt.MapFrom(sec => sec.Photos.Image))
        .ReverseMap();
        #endregion
    }
}