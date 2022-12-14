using AutoMapper;
using E_Books.IServices;
using E_Books.Models;
using E_Books.ViewModel;
using E_Books.ViewModel.ToView;
using E_Books.ViewModel.FromView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]/[action]")]
public class AdminBooksController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;

    private long _maxSizeImage = 1048576;
    private List<string> _allowedExtensions = new() { ".png", ".jpg" };
    public AdminBooksController(IUnitOfWork service, IMapper mapper) => (_service, _mapper) = (service, mapper);

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



    [HttpGet]
    public IActionResult GetCounts() => Ok(_service.Book.GetCount());



    [HttpGet("get-publishers")]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await _service.Publisher.GetAllAsync(expression: null);
        var response = _mapper.Map<IEnumerable<PublisherVM>>(publishers);
        return Ok(response);
    }



    [HttpGet("get-languages")]
    public async Task<IActionResult> GetAllLanguages()
    {
        var languages = await _service.Language.GetAllAsync(expression: null);
        var response = _mapper.Map<IEnumerable<LanguageVM>>(languages);
        return Ok(response);
    }



    [HttpGet("get-genres")]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await _service.Genre.GetAllAsync(expression: null);
        var response = _mapper.Map<IEnumerable<GenreVM>>(genres);
        return Ok(response);
    }



    [HttpPost]
    public async Task<IActionResult> AddBookAsync([FromForm] BookVM bookVM)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_allowedExtensions.Contains(Path.GetExtension(bookVM.Image.FileName).ToLower()))
            return BadRequest("Only .png and jpg images are allowed!");

        if (bookVM.Image.Length > _maxSizeImage)
            return BadRequest("Max allowed size for Image book is 1MB!");

        var book = _mapper.Map<BookVM, Book>(bookVM);

        book.PublicationDate = DateTime.UtcNow;

        await _service.Book.AddAsync(book);
        await _service.SaveAsync();

        var readBook = await _service.Book.GetBookAsync(book.Id, true);

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
        var book = await _service.Book.GetAsync(expression: b => b.Id == id, include: null);

        _service.Book.Delete(book);

        await _service.SaveAsync();

        return NoContent();
    }
}
