using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Books.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]

public class AdminAuthorsController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;

    public AdminAuthorsController(IUnitOfWork service,
                                  IMapper mapper) => (_service, _mapper) = (service, mapper);




    [Authorize]
    [HttpGet("get-all-authors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _service.Author.GetAllAsync(predicate: a => a.Id > 0, include => include.Include(book => book.Books).ThenInclude(bookAuthor => bookAuthor.Books));
        if(authors.Count == 0)
        return NotFound("There are no authors yet");

        return Ok(_mapper.Map<IEnumerable<AuthorBooksVM>>(authors));
    }

    [Authorize]
    [HttpGet("get-author-by-id/{authorId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthorAsync(int authorId)
    {
        var author = await _service.Author.GetAsync(predicate: author => author.Id == authorId, null);

        if (author is null)
            return NotFound();

        var response = _mapper.Map<KeyResource>(author);

        return Ok(response);
    }


    [Authorize]
    [HttpGet("get-author-with-books/{authorId:int}", Name = "GetAuthorWithBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthorWithBooks(int authorId)
    {
        var author = await _service.Author.GetAsync(predicate: author => author.Id == authorId,include => include.Include(book => book.Books).ThenInclude(b => b.Books));

        if (author is null)
            return NotFound();

        return Ok(_mapper.Map<Author, AuthorBooksVM>(author));
    }

    [Authorize]
    [HttpGet("get-all-authors-with-books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAuthorsWithBooks()
    {
        var authors = await _service.Author.GetAllAsync(predicate: a => a.Id > 0, include: inc => inc.Include(b => b.Books).ThenInclude(ba => ba.Books));

        return Ok(_mapper.Map<IEnumerable<AuthorBooksVM>>(authors));
    }



    [Authorize(Roles = "Admin")]
    [HttpPost("add-author")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAuthor([FromBody] KeyResource model)
    {
        var author = _mapper.Map<KeyResource, Author>(model);

        await _service.Author.AddAsync(author);
        await _service.SaveAsync();

        var response = _mapper.Map<KeyResource>(author);

        return Created(nameof(AddAuthor), new { response, status = StatusCodes.Status201Created });
    }




    [Authorize(Roles = "Admin")]
    [HttpPut("update-author/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] KeyResource model)
    {
        if (!ModelState.IsValid)
            return BadRequest($"Submitted data is invalid ,{ModelState}");
        var author = await _service.Author.GetAsync(predicate: idauth => idauth.Id == id, null);

        if (author is null)
            return NotFound();

        _mapper.Map(model, author);
        _service.Author.Update(author);

        await _service.SaveAsync();

        return NoContent();
    }



    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-author/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthorAsync(int id)
    {
        var author = await _service.Author.GetAsync(predicate: author => author.Id == id, null);

        _service.Author.Delete(author);
        await _service.SaveAsync();

        return NoContent();
    }

}
