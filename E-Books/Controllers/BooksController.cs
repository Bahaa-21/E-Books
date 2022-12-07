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

namespace E_Books.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;
    public BooksController(IUnitOfWork service, IMapper mapper) => (_service, _mapper) = (service, mapper);


    [HttpGet("get-all-books")]
    public async Task<IActionResult> GetAllBookAsync([FromQuery] RequestParams requestParams)  
    {
        var books = await _service.Book.GetAllBookAsync(requestParams);
        
        var response = _mapper.Map<IEnumerable<ReadBookVM>>(books);
        return Ok(response);
    }


    [HttpGet("get-book-by-id/{id:int}")]

    public async Task<IActionResult> GetBookByIdAsync(int id)
    {
        var book = await _service.Book.GetBookAsync(id , true);
        if (book is null)
            return NotFound();


        var response = _mapper.Map<Book, ReadBookVM>(book);
        return Ok(response);
    }


    [HttpPost("add-book")]
    public async Task<IActionResult> AddBookAsync([FromBody] BookVM bookVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var book = _mapper.Map<BookVM, Book>(bookVM);
        book.PublicationDate = DateTime.UtcNow;
        await _service.Book.AddAsync(book);
        await _service.SaveAsync();

        var response = _mapper.Map<Book, BookVM>(book);

        return Created(nameof(AddBookAsync), response);
    }


    [HttpPut("update-book/{id:int}")]
    public async Task<IActionResult> UpdateBookAsync(int id, [FromBody] BookVM updateBook)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");

        var book = await _service.Book.GetAsync<Book>(ex => ex.Id == id);
        
        if (book is null)
            return NotFound();

        _mapper.Map(updateBook, book);

        _service.Book.Update(book);

        await _service.SaveAsync();

        return NoContent();
    }

   [HttpDelete("delete-book/{id:int}")]
   public async Task<IActionResult> DeleteBookAsync(int id)
   {
        var book = await _service.Book.GetAsync<Book>(i => i.Id == id);

        _service.Book.Delete<Book>(book);

        await _service.SaveAsync();

        return NoContent();
   }
}
