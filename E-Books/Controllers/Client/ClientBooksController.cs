using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Books.Data;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace E_Books.Controllers.Client;

[ApiController]
[Route("api/[controller]")]

public class ClientBooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly UserManager<UsersApp> _userManager;
    private readonly IBookService _bookService;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    private readonly IMapper _mapper;
    public ClientBooksController(IUnitOfWork service, IBookService bookService, IMapper mapper, IAuthService authService, UserManager<UsersApp> userManager, IUserService userService) =>
    (_service, _bookService, _mapper, _authService, _userManager ,_userService) = (service, bookService, mapper, authService, userManager,userService);


    [HttpGet("get-all-books")]
    public async Task<IActionResult> GetAllBookAsync([FromQuery] RequestParams requestParams)
    {
        var books = await _bookService.GetAllBookAsync(requestParams);

        int tatalPage = books.PageCount;

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);

        return Ok(new { response, tatalPage });
    }


    [HttpGet("get-book-by-id/{id:int}")]
    public async Task<IActionResult> GetBookByIdAsync(int id)
    {
        var book = await _bookService.GetBookAsync(id, true);

        if (book is null)
            return NotFound();

        var response = _mapper.Map<Book, ReadBookVM>(book);

        return Ok(response);
    }


    [HttpGet("get-book-by-genre/{id}")]
    public async Task<IActionResult> GetBookByGenre(int id)
    {
        var books = await _bookService.GetBookGenre(id);

        if (books is null)
            return NotFound();

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);
        return Ok(response);
    }


    [HttpGet("search-of-books")]
    public async Task<IActionResult> SearchAsync(string title, [FromQuery] RequestParams requestParams)
    {
        var book = await _service.Book.Search(requestParams, predicate: book => book.Title.Contains(title),
                                                                include: inc => inc.Include(author => author.Authors).ThenInclude(bookAuthor => bookAuthor.Authors));
        if (book.Count == 0)
            return NotFound($"Sorry, this title : {title}, does't exist, Please try agin");

        var response = _mapper.Map<IEnumerable<SearchBookVM>>(book);

        return Ok(response);
    }



    [Authorize(Roles = "User")]
    [HttpGet("get-user-profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userProfile = await _userService.GetUserProfile();

        var response = _mapper.Map<UserProfileVM>(userProfile);

        return Ok(response);
    }




    [Authorize(Roles = "User")]
    [HttpPatch("update-user-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var user = await _userService.GetUserProfile();

        if (user is null)
            return Unauthorized();

        _mapper.Map(model, user);

        var result = await _userService.UpdateProfile(user);

        if (!result)
            return BadRequest(user);

        var response = _mapper.Map<UpdateProfileVM>(user); 

        return Ok(response);
    }

}
