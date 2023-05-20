using AutoMapper;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.EntityFrameworkCore;
using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Books.Helper;

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
        .ForMember(d => d.Name, opt => opt.MapFrom(sec => sec.LanguageName))
        .ReverseMap();
        #endregion


        #region Book Map
        CreateMap<Book, BookVM>()
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.AuthorId)))
        .ReverseMap()
        .ForMember(b => b.Authors, opt => opt.Ignore())
        .AfterMap((bookVm, book) =>
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

        CreateMap<Book, BookDetailsVM>()
        .ForMember(d => d.Publishers, opt => opt.MapFrom(sec => sec.Publishers.Name))
        .ForMember(d => d.Price, opt => opt.MapFrom(sec => sec.Price.ToString("c")))
        .ForMember(d => d.PublicationDate, opt => opt.MapFrom(sec => sec.PublicationDate.ToString("f")))
        .ForMember(d => d.Language, opt => opt.MapFrom(sec => sec.Languages.LanguageName))
        .ForMember(d => d.GenreType, opt => opt.MapFrom(sec => sec.Genres.Name))
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors))
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(author => author.Authors.Name).ToList()))
        .ReverseMap();

        CreateMap<Book, SearchBookVM>()
        .ForMember(d => d.BookImage, opt => opt.MapFrom(sec => sec.Image))
        .ForMember(d => d.Price, opt => opt.MapFrom(sec => sec.Price.ToString("c")))
        .ForMember(d => d.Authors, opt => opt.MapFrom(sec => sec.Authors.Select(a => a.Authors.Name).ToList()))
        .ReverseMap();
        #endregion 


        #region User Map
        CreateMap<UsersApp, UserProfileVM>()
        .ForMember(des => des.ProfilePhoto, opt => opt.MapFrom(sec => sec.Photo))
        .AfterMap((user, userVM) =>
        {
            if (userVM.ProfilePhoto is not null)
                userVM.ProfilePhoto = "data:image/png;base64," + Convert.ToBase64String(user.Photo.Image);
            else
                userVM.ProfilePhoto = null;
        })
        .ReverseMap();


        CreateMap<UsersApp, UpdateProfileVM>().ReverseMap();
        #endregion


        #region Role Map
        CreateMap<IdentityRole, RolesVM>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(sec => sec.Id))
            .ReverseMap();
        #endregion


        #region Cart Map
        CreateMap<CartBook, CartsDetailsVM>()
        .ForMember(d => d.BookId, opt => opt.MapFrom(sec => sec.Books.Id))
        .ForMember(d => d.BookName, opt => opt.MapFrom(sec => sec.Books.Title))
        .ForMember(d => d.BookPhoto, opt => opt.MapFrom(sec => sec.Books.Image))
        .ForMember(d => d.Price, opt => opt.MapFrom(sec => sec.Books.Price.ToString("c")))
        .ForMember(d => d.Amount, opt => opt.MapFrom(sec => sec.Amount))
        .ForMember(d => d.AddedOn, opt => opt.MapFrom(sec => sec.AddedOn.ToString("f")));

        CreateMap<Carts, CartsVM>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(sec => sec.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(sec => sec.Users.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(sec => sec.Users.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(sec => sec.Users.Address))
            .ReverseMap();


        CreateMap<CartBook, RemoveItemCartVM>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(sec => sec.Books.Title))
            .ReverseMap();
        #endregion


        #region Order Map
        CreateMap<Order, OrderItemsVM>()
        .ForMember(d => d.Created, opt => opt.MapFrom(sec => sec.Created.ToString("f")))
        .ForMember(d => d.Books, opt => opt.MapFrom(sec => sec.OrderItems))
        .ForMember(d => d.Books, opt => opt.MapFrom(sec => sec.OrderItems.Select(s => new
        {
            Title = s.Books.Title,
            Price = s.Books.Price.ToString("c"),
            Quantity = s.Amount,
            TotalPrice = (s.Price * s.Amount).ToString("c")
        })))
        .ForMember(d => d.TotalPice, opt => opt.MapFrom(sec => sec.OrderItems.Select(s => s.Amount * s.Price).Sum().ToString("C")));
        #endregion
    }
}