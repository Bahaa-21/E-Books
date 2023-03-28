using AutoMapper;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.DataAccessLayer.Models;
using E_Books.ViewModel.FromView;
using E_Books.ViewModel.ToView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Books.Controllers.Admin;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
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


    [HttpPost("add-publisher")]
    public async Task<IActionResult> AddPublisher([FromBody] KeyResource model)
    {
        var publisher = _mapper.Map<KeyResource, Publisher>(model);

        await _service.Publisher.AddAsync(publisher);
        await _service.SaveAsync();
        
        return Created(nameof(AddPublisher),_mapper.Map<KeyResource>(publisher));
    } 
}
