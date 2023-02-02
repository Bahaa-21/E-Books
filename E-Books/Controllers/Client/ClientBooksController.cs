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

namespace E_Books.Controllers.Client;

[ApiController]
[Route("api/[controller]")]
public class ClientBooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;
    public ClientBooksController(IUnitOfWork service, IMapper mapper) => (_service, _mapper) = (service, mapper);


    [HttpGet("get-all-books")]
    public async Task<IActionResult> GetAllBookAsync([FromQuery] RequestParams requestParams)
    {
        var books = await _service.Book.GetAllAsync(requestParams, include: inc => inc.Include(a => a.Authors).ThenInclude(ab => ab.Authors)
                                                   .Include(p => p.Publishers)
                                                   .Include(l => l.Languages)
                                                   .Include(g => g.Genres));

        int  pageNumber = _service.Book.PageNumber(books.Count());

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);

        return Ok(new{response , pageNumber });
    }


    [HttpGet("get-book-by-id/{id:int}")]
    public async Task<IActionResult> GetBookByIdAsync(int id)
    {
        var book = await _service.Book.GetAsync(predicate : bookId => bookId.Id == id, include: inc => inc.Include(a => a.Authors).ThenInclude(ab => ab.Authors)
                                                        .Include(p => p.Publishers)
                                                        .Include(l => l.Languages)
                                                        .Include(g => g.Genres));
        if (book is null)
            return NotFound();

        var response = _mapper.Map<Book, ReadBookVM>(book);

        return Ok(response);
    }


    [HttpGet("get-book-by-genre/{id:int}")]
    public async Task<IActionResult> GetBookByGenre(int id)
    {
        var books = await _service.Book.GetAllAsync(predicate : g => g.GenreId == id,
                                                    include: inc => inc.Include(author => author.Authors)
                                                    .ThenInclude(ab => ab.Authors)
                                                    .Include(publisher => publisher.Publishers)
                                                    .Include(language => language.Languages)
                                                    .Include(genre => genre.Genres));
        if (books is null)
            return NotFound();

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);
        return Ok(response);
    }


    [HttpGet("search-of-books")]
    public async Task<IActionResult> SearchAsync(string title, [FromQuery] RequestParams requestParams)
    {
        var book = await _service.Book.Search(requestParams, predicate : book => book.Title.Contains(title),
                                                                include: inc => inc.Include(author => author.Authors).ThenInclude(bookAuthor => bookAuthor.Authors));
        if (book.Count == 0)
            return NotFound($"Sorry, this title : {title}, does't exist, Please try agin");

        var response = _mapper.Map<IEnumerable<SearchBookVM>>(book);

        return Ok(response);
    }

}
