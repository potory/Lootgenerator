using AutoMapper;
using LootGenerator.Contracts.Requests.Items;
using LootGenerator.Contracts.Responses.Items;
using LootGenerator.Data;
using LootGenerator.Models;
using LootGenerator.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LootGenerator.Controllers;

public class ItemController : Controller
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly DiceUtility _diceUtility;

    public ItemController(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _diceUtility = new DiceUtility();
    }
    
    public async Task<IActionResult> Index()
    {
        var items = await _context.Items.ToListAsync();

        var response = _mapper.Map<List<Item>, List<GetItemResponse>>(items);

        return View(response);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var cost = request.Cost.ToLower();
        
        if (!_diceUtility.IsCorrectDiceString(cost))
        {
            ModelState.AddModelError(nameof(request.Cost), "Значение цены не соответствует формату кубика");
            return View(request);
        }

        request.Cost = cost;
        
        var item = _mapper.Map<PostItemRequest, Item>(request);

        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit([FromQuery] int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item is null)
        {
            return NotFound();
        }
        
        var form = _mapper.Map<Item, PutItemRequest>(item);
        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] PutItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        request.Cost = request.Cost.ToLower();

        var item = _mapper.Map<PutItemRequest, Item>(request);
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}