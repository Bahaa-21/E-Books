using AutoMapper;
using E_Books.IServices;
using E_Books.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]/[action]")]
public class AdminAuthorsController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;

    public AdminAuthorsController(IUnitOfWork service, IMapper mapper) => (_service, _mapper) = (service, mapper);


    [HttpGet("get-all-authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _service.Author.GetAllAsync(expression: null,
                                                        include: inc => inc.Include(book => book.Books)
                                                                            .ThenInclude(bookAuthor => bookAuthor.Books));

        return Ok(_mapper.Map<IEnumerable<BooksAuthorVM>>(authors));
    }


    [HttpGet("get-author-by-id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthorAsync(int id)
    {
        var author = await _service.Author.GetAsync(expression: authorId => authorId.Id == id, null);

        if (author is null)
            return NotFound();

        var response = _mapper.Map<ReadAuthorVM>(author);

        return Ok(response);
    }


    [HttpGet("get-author-with-books/{id:int}", Name = "GetAuthorWithBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthorWithBooks(int id)
    {
        var author = await _service.Author.GetAsync(expression: ai => ai.Id == id, include: inc => inc.Include(book => book.Books)
        .ThenInclude(b => b.Books));

        if (author is null)
            return NotFound();

        return Ok(_mapper.Map<Author, BooksAuthorVM>(author));
    }


    [HttpGet("get-all-authors-with-books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAuthorsWithBooks()
    {
        var authors = await _service.Author.GetAllAsync(expression: null, include: inc => inc.Include(b => b.Books).ThenInclude(ba => ba.Books));

        return Ok(_mapper.Map<IEnumerable<BooksAuthorVM>>(authors));
    }


    [HttpPost("add-author")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorVM authorVM)
    {
        var author = _mapper.Map<AuthorVM, Author>(authorVM);

        await _service.Author.AddAsync(author);
        await _service.SaveAsync();

        var response = _mapper.Map<AuthorVM>(author);

        return Created(nameof(AddAuthor),  new{ response , status = StatusCodes.Status201Created });
    }


    [HttpPut("update-author")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorVM authorVM)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");
        var author = await _service.Author.GetAsync(expression: id => id.Id == authorVM.Id, null);

        if (author is null)
            return NotFound();

        _mapper.Map(authorVM, author);
        _service.Author.Update(author);
        await _service.SaveAsync();

        return NoContent();
    }


    [HttpDelete("delete-author/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthorAsync(int id)
    {
        var author = await _service.Author.GetAsync(expression: authorId => authorId.Id == id, null);

        _service.Author.Delete(author);
        await _service.SaveAsync();

        return NoContent();
    }

}
