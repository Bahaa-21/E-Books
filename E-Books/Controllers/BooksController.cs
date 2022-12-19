using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using E_Books.Data;
using E_Books.ViewModel;
using E_Books.IServices;
using E_Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace E_Books.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;

    private long _maxSizeImage = 1048576;
    private List<string> _allowedExtensions = new () {".png" ,".jpg"};
    public BooksController(IUnitOfWork service, IMapper mapper) => (_service, _mapper) = (service, mapper);


    [HttpGet]
    public async Task<IActionResult> GetAllBookAsync([FromQuery] RequestParams requestParams)
    {
        var books = await _service.Book.GetAllAsync(requestParams, include: inc => inc.Include(a => a.Authors).ThenInclude(ab => ab.Authors)
                                                   .Include(p => p.Publishers)
                                                   .Include(l => l.Languages)
                                                   .Include(g => g.Genres));

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);

        return Ok(response);
    }


    [HttpGet("{id:int}" , Name = "GetBookByIdAsync")]

    public async Task<IActionResult> GetBookByIdAsync(int id)
    {
        var book = await _service.Book.GetBookAsync(id , true);
        if (book is null)
            return NotFound();

        var response = _mapper.Map<Book, ReadBookVM>(book);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookByGenre(int id)
    {
        var books = await _service.Book.GetAllAsync(expression: g => g.GenreId == id,
                                                    include: inc => inc.Include(a => a.Authors)
                                                    .ThenInclude(ab => ab.Authors)
                                                    .Include(p => p.Publishers)
                                                    .Include(l => l.Languages)
                                                    .Include(g => g.Genres));
        if (books is null)
            return NotFound();

        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> SearchAsync(string title)
    {
        var book = await _service.Book.GetAllAsync(expression: book => book.Title.Contains(title),
                                                    include: inc => inc.Include(a => a.Authors).ThenInclude(ab => ab.Authors));

        if (book.Count == 0)
            return NotFound($"Sorry, this title : {title}, does't exist, Please try agin");

        var response = _mapper.Map<IEnumerable<SearchBookVM>>(book);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookAsync([FromForm] BookVM bookVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!_allowedExtensions.Contains(Path.GetExtension(bookVM.Image.FileName).ToLower()))
            return BadRequest("Only .png and jpg images are allowed!");

        if(bookVM.Image.Length > _maxSizeImage)
            return BadRequest("Max allowed size for Image book is 1MB!");

        var book = _mapper.Map<BookVM, Book>(bookVM);

        book.PublicationDate = DateTime.UtcNow;

        await _service.Book.AddAsync(book);
        await _service.SaveAsync();

        var readBook = await _service.Book.GetBookAsync(book.Id , true);

        var response = _mapper.Map<ReadBookVM>(readBook);

        return Created(nameof(AddBookAsync), response);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBookAsync(int id, [FromBody] BookVM updateBook)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");

        var book = await _service.Book.GetAsync(expression: b => b.Id == id, null);

        if (book is null)
            return NotFound();

        _mapper.Map(updateBook, book);

        _service.Book.Update(book);

        await _service.SaveAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookAsync(int id)
    {
        var book = await _service.Book.GetAsync(expression: b => b.Id == id,include : null);

        _service.Book.Delete(book);

        await _service.SaveAsync();

        return NoContent();
    }
}
