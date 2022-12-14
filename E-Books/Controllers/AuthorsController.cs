using AutoMapper;
using E_Books.IServices;
using E_Books.Models;
using E_Books.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;

    public AuthorsController(IUnitOfWork service, IMapper mapper) => (_service, _mapper) = (service, mapper);

    [HttpGet("get-all-authors")]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _service.Author.GetAllAsync(expression: null);
        return Ok(_mapper.Map<IEnumerable<AuthorVM>>(authors));
    }

    [HttpGet("get-author-by-id/{id:int}")]

    public async Task<IActionResult> GetAuthorAsync(int id)
    {
        var author = await _service.Author.GetAsync(expression : a => a.Id == id , null);

        if (author is null)
            return NotFound();

        var response = _mapper.Map<AuthorVM>(author);

        return Ok(response);
    }

}
