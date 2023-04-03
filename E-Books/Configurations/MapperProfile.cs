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
        CreateMap<Publisher, KeyResource>()
        .ReverseMap();

        CreateMap<Publisher, PublisherBooksVM>()
        .ForMember(d => d.BookTitle, opt => opt.MapFrom(sec => sec.Books.Select(t => t.Title)))
       .ReverseMap();
        #endregion

        #region Photo Map
        CreateMap<Photo , DisplayPhotoVM>()
        .ForMember(d => d.ProfilePhoto , opt => opt.MapFrom(sec => sec.Image))
        .ForMember(d => d.AddedOn , opt => opt.MapFrom(sec => sec.CreatedOn.ToString("f")))
        .ReverseMap();
        #endregion

        #region Genre Map
        CreateMap<Genre, KeyResource>().ReverseMap();
        #endregion


        #region Author Map
        CreateMap<Author, KeyResource>().ReverseMap();

        CreateMap<Author, AuthorBooksVM>()
        .ForMember(d => d.BookTitle, opt => opt.MapFrom(sec => sec.Books))
        .ForMember(d => d.BookTitle, opt => opt.MapFrom(sec => sec.Books.Select(b => new
        {
            Id = b.Books.Id,
            Title = b.Books.Title
        }).ToList()))
        .ReverseMap();

        #endregion


        #region Language Map
        CreateMap<BookLanguage, KeyResource>()
        .ForMember(d => d.Name , opt => opt.MapFrom(sec => sec.LanguageName))
        .ReverseMap();
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
        .ForMember(d => d.Price, opt => opt.MapFrom(sec => sec.Price.ToString("c")))
        .ForMember(d => d.PublicationDate, opt => opt.MapFrom(sec => sec.PublicationDate.ToString("f")))
        .ForMember(d => d.Language , opt => opt.MapFrom(sec => sec.Languages.LanguageName))
        .ForMember(d => d.GenreType , opt => opt.MapFrom(sec => sec.Genres.Name))
        .ForMember(d => d.Authors , opt => opt.MapFrom(sec => sec.Authors))
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(author => author.Authors.Name).ToList()))
        .ReverseMap();
       
        CreateMap<Book, SearchBookVM>()
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.Authors.Name).ToList()))
        .ReverseMap();
        #endregion 


        #region User Map
        CreateMap<UsersApp , UserProfileVM>()
        .ForMember(des => des.ProfilePhoto , opt => opt.MapFrom(sec => sec.Photos.Image))
        .ReverseMap();
        
        CreateMap<UsersApp , UpdateProfileVM>().ReverseMap();
        #endregion


        #region Cart Map
        CreateMap<CartBook , CartsVM>()
        .ForMember(d => d.BookId , opt => opt.MapFrom(sec => sec.Books.Id))
        .ForMember(d => d.BookName , opt => opt.MapFrom(sec => sec.Books.Title))
        .ForMember(d => d.BookPhoto , opt => opt.MapFrom(sec => sec.Books.Image))
        .ForMember(d => d.Price , opt => opt.MapFrom(sec => sec.Books.Price.ToString("c")))
        .ForMember(d => d.Amount , opt => opt.MapFrom(sec => sec.Amount))
        .ForMember(d => d.AddedOn , opt => opt.MapFrom(sec => sec.AddedOn.ToString("f")));
        #endregion

        #region Order Map
        CreateMap<Order , OrderItemsVM>()
        .ForMember(d => d.Id , opt => opt.MapFrom(sec => sec.Id))
        .ForMember(d => d.Created , opt => opt.MapFrom(sec => sec.Created.ToString("f")))
        .ForMember(d => d.Book , opt => opt.MapFrom(sec => sec.OrderItems))
        .ForMember(d => d.Book , opt => opt.MapFrom(sec => sec.OrderItems.Select(s => new
        {
            title = s.Books.Title,
            price = s.Books.Price.ToString("c"),
            qty = s.Amount,
            totalPrice = (s.Price * s.Amount).ToString("c")
        })))
        .ForMember(d => d.TotalPice , opt => opt.MapFrom(sec => sec.OrderItems.Select(s => s.Amount * s.Price).Sum().ToString("C")));
        #endregion
    }
}