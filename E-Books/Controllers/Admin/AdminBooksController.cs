using AutoMapper;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminBooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;
    private readonly IBookService _bookService;
    private long _maxSizeImage = 1048576;
    public AdminBooksController(IUnitOfWork service,
                                IMapper mapper,
                                IBookService bookService) => 
                                (_service, _mapper, _bookService) = (service, mapper, bookService);



    [HttpGet("get-webApp-info")]
    public IActionResult GetCounts() => Ok(_service.Book.GetCount());



    [HttpGet("get-publishers")]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await _service.Publisher.GetAllAsync(predicate: null);
        var response = _mapper.Map<IEnumerable<PublisherVM>>(publishers);
        return Ok(response);
    }



    [HttpGet("get-languages")]
    public async Task<IActionResult> GetAllLanguages()
    {
        var languages = await _service.Language.GetAllAsync(predicate: null);
        var response = _mapper.Map<IEnumerable<LanguageVM>>(languages);
        return Ok(response);
    }



    [HttpGet("get-genres")]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await _service.Genre.GetAllAsync(predicate: null);
        var response = _mapper.Map<IEnumerable<GenreVM>>(genres);

        return Ok(response);
    }



    
    [HttpPost("add-book")]
    public async Task<IActionResult> AddBookAsync([FromBody] BookVM bookVM)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (bookVM.Image.Length > _maxSizeImage)
            return BadRequest("Max allowed size for Image book is 1MB!");



        var book = _mapper.Map<Book>(bookVM);


        await _service.Book.AddAsync(book);
        await _service.SaveAsync();

        var readBook = await _bookService.GetBookAsync(book.Id, true);
        var response = _mapper.Map<ReadBookVM>(readBook);

        return Created(nameof(AddBookAsync), response);
    }




    
    [HttpPut("update-book/{id}")]
    public async Task<IActionResult> UpdateBookAsync(int id, [FromBody] BookVM updateBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var book = await _bookService.GetBookWithAuthor(id);

        if (book is null)
            return NotFound();

        _mapper.Map<BookVM, Book>(updateBook, book);

        _service.Book.Update(book);

        await _service.SaveAsync();

        return NoContent();
    }




    
    [HttpDelete("delete-book/{id}")]
    public async Task<IActionResult> DeleteBookAsync(int id)
    {
        var book = await _service.Book.GetAsync(predicate: b => b.Id == id, include: null);

        _service.Book.Delete(book);

        await _service.SaveAsync();

        return NoContent();
    }
}
