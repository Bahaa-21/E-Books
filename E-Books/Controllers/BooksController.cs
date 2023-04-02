using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _service;
    public BooksController(IUnitOfWork service, IBookService bookService, IMapper mapper)
    {
        this._service = service;
        this._mapper = mapper;
        this._bookService = bookService;
    }
    

    [HttpGet("get-all-books")]
    public async Task<IActionResult> GetAllBookAsync([FromQuery] RequestParams requestParams)
    {
        var books = await _bookService.GetAllBookAsync(requestParams);

        var metaData = new MetaData(books.PageCount, books.PageNumber);

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);

        return Ok(new { response, metaData });
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
    public async Task<IActionResult> GetBookByGenre(int id, [FromQuery] RequestParams requestParams)
    {
        var books = await _bookService.GetBookGenre(id, param: requestParams);

        if (books.Count == 0)
            return NotFound();

        var metaData = new MetaData(books.PageCount, books.PageNumber);

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);
        return Ok(new { response, metaData });
    }


    [HttpGet("search-of-books")]
    public async Task<IActionResult> SearchAsync(string title, [FromQuery] RequestParams requestParams)
    {
        var book = await _service.Book.Search(requestParams, include: inc => inc.Include(author => author.Authors).ThenInclude(bookAuthor => bookAuthor.Authors),
        predicate: book => book.Title.Contains(title));
        if (book.Count == 0)
            return NotFound($"Sorry, this title : {title}, does't exist, Please try agin");

        var response = _mapper.Map<IEnumerable<SearchBookVM>>(book);

        return Ok(response);
    }

    private record MetaData(int TotalPage, int PageNumber);
}