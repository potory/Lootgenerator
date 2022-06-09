using AutoMapper;
using LootGenerator.Contracts;
using LootGenerator.Contracts.Requests;
using LootGenerator.Contracts.Responses;
using LootGenerator.Data;
using LootGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace LootGenerator.Controllers;

[ApiController]
public class ItemApiController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ItemApiController(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet(ApiRoutes.Item.Get)]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        var response = _mapper.Map<GetItemResponse>(item);

        return Ok(response);
    }
    
    [HttpPost(ApiRoutes.Item.Post)]
    public async Task<IActionResult> Get([FromBody] PostItemRequest request)
    {
        var item = _mapper.Map<Item>(request);

        await _context.AddAsync(item);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<GetItemResponse>(item));
    }
    
    [HttpPut(ApiRoutes.Item.Put)]
    public async Task<IActionResult> Put([FromBody] PutItemRequest request)
    {
        var item = _mapper.Map<Item>(request);

        _context.Update(item);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<GetItemResponse>(item));
    }
    
    [HttpDelete(ApiRoutes.Item.Delete)]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }
}