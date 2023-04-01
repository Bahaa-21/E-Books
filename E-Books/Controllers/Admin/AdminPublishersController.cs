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
[Authorize(Roles = "Admin")]
public class AdminPublishersController : ControllerBase
{
     private readonly IUnitOfWork _service;
    private readonly IMapper _mapper;

    public AdminPublishersController(IUnitOfWork service,
                                  IMapper mapper) => (_service, _mapper) = (service, mapper);


    [HttpGet("get-publishers")]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await _service.Publisher.GetAllAsync(p => p.Id > 0);
        if (publishers.Count == 0)
            return NotFound("There are no publishers yet");
            
        var response = _mapper.Map<IEnumerable<KeyResource>>(publishers);
        return Ok(response);
    }


    [HttpGet("get-publisher-books")]
    public async Task<IActionResult> GetPublisherBooks()
    {
       var publishers = await _service.Publisher.GetAllAsync(predicate: p => p.Id > 0, include: inc => inc.Include(b => b.Books));
        if (publishers is null)
            return NotFound();

        return Ok(_mapper.Map<IEnumerable<PublisherBooksVM>>(publishers));  
    }



    [HttpPost("add-publisher")]
    public async Task<IActionResult> AddPublisher([FromBody] KeyResource model)
    {
        var publisher = _mapper.Map<KeyResource, Publisher>(model);

        await _service.Publisher.AddAsync(publisher);
        await _service.SaveAsync();
        
        return Created(nameof(AddPublisher),_mapper.Map<KeyResource>(publisher));
    }
}
