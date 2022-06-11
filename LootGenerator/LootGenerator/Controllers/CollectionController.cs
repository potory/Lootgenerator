using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using CsvHelper;
using LootGenerator.Contracts.Requests.Collections;
using LootGenerator.Contracts.Requests.Items;
using LootGenerator.Contracts.Responses.Collections;
using LootGenerator.Data;
using LootGenerator.Extensions;
using LootGenerator.Models;
using LootGenerator.Models.ModelViews.Collection;
using LootGenerator.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LootGenerator.Controllers;

public class CollectionController : Controller
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly DiceUtility _dice;

    public CollectionController(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dice = new DiceUtility();
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Collections.ToListAsync());
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostCollectionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var item = _mapper.Map<PostCollectionRequest, ItemCollection>(request);

        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromQuery] int id)
    {
        var collection = await _context.Collections.Include(x => x.Records)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (collection is null)
        {
            return NotFound();
        }

        return View(collection);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var collection = await _context.Collections.FindAsync(id);
        
        if (collection == null)
        {
            return NotFound();
        }

        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ItemCollection collection)
    {
        if (!ModelState.IsValid)
        {
            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
            return BadRequest($"Проверьте правильность заполнения полей:\n{string.Join('\n', err)}");
        }

        var collectionInstance = await _context.Collections.FindAsync(collection.Id);

        if (collectionInstance is null)
        {
            return NotFound();
        }

        collection.Records ??= new List<ItemCollectionRecord>();

        collectionInstance.Name = collection.Name;
        collectionInstance.Description = collection.Description;

        var existingRecords = await _context.CollectionRecords.Where(x => x.CollectionId == collectionInstance.Id).AsNoTracking().ToListAsync();

        for (var i = existingRecords.Count-1; i >= 0; i--)
        {
            var curRecord = existingRecords[i];
            var updRecord = collection.Records.FirstOrDefault(record => record.RecordId == curRecord.RecordId);

            if (updRecord is null)
            {
                _context.CollectionRecords.Remove(curRecord);
                continue;
            }

            _context.CollectionRecords.Update(updRecord);
        }

        var newRecords = collection.Records.Where(record => record.RecordId == 0);
        
        foreach (var record in newRecords)
        {
            record.CollectionId = collectionInstance.Id;
        }
        
        await _context.CollectionRecords.AddRangeAsync(newRecords);
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    public async Task<IActionResult> Randomize(int id)
    {
        var collection = await _context.Collections
            .Include(collection => collection.Records)
            .ThenInclude(x => x.Item)
            .FirstOrDefaultAsync(collection => collection.Id == id);

        if (collection is null)
        {
            return NotFound();
        }

        var rand = new Random();
        
        var response = new RandomizeCollectionResponse
        {
            Name = collection.Name,
            Description = collection.Description
        };

        foreach (var record in collection.Records)
        {
            var chance = rand.NextSingle();

            if (record.Chance <= chance)
            {
                continue;
            }

            var price = _dice.Roll(record.Item.Cost, out var priceCalcs);
            var count = _dice.Roll(record.Count, out var countCalcs);
            
            var responseRecord = new RandomizeCollectionResponse.ItemRecord
            (
                id: record.Item.Id,
                name: record.Item.Name,
                price: $"{record.Item.Cost} = {priceCalcs} = {price}",
                count: $"{record.Count} = {countCalcs} = {count}"
                );
            
            response.Records.Add(responseRecord);
        }

        return View(response);
    }
    
    [HttpGet]
    public IActionResult BlankItemRecord()
    {
        return PartialView("PartialViews/_ItemCollectionRecordEditor", new ItemCollectionRecord());
    }

    [HttpGet]
    public async Task<IActionResult> Upload()
    {
        var model = new CollectionUploadViewModel();

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(CollectionUploadViewModel upload)
    {
        if (!ModelState.IsValid)
        {
            return View(upload);
        }
        
        if (upload.File == null || upload.File.Length == 0)
        {
            ModelState.AddModelError(nameof(upload.File), "Файл не может быть пустым");
            return View(upload);
        }

        var text = await ReadAsStringAsync(upload.File);
        text = text.Replace(';', ',');

        List<UploadCollectionRecord> dtos;

        using (var stringReader = new StringReader(text))
        using (var csv = new CsvReader(stringReader, CultureInfo.InvariantCulture))
        {
            dtos = csv
                .GetRecords<UploadCollectionRecord>()
                .Where(x => 
                    !string.IsNullOrEmpty(x.Name) &&
                    !string.IsNullOrEmpty(x.Price) &&
                    !string.IsNullOrEmpty(x.Count) &&
                    !string.IsNullOrEmpty(x.Chance))
                .ToList();
        }

        var collection = new ItemCollection
        {
            Name = upload.Name,
            Records = new List<ItemCollectionRecord>()
        };

        foreach (var dto in dtos)
        {
            var price = ConvertDice(dto.Price);

            var item = new Item
            {
                Name = dto.Name,
                Cost = price
            };
            
            var chance = ConvertPercents(dto.Chance);
            var count = ConvertDice(dto.Count);

            var record = new ItemCollectionRecord()
            {
                Chance = chance,
                Count = count,
                Item = item
            };

            collection.Records.Add(record);
        }

        await _context.AddAsync(collection);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Edit), new {id = collection.Id});
    }

    private static async Task<string> ReadAsStringAsync(IFormFile file)
    {
        var result = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
            {
                result.AppendLine(await reader.ReadLineAsync());
            }
        }
        
        return result.ToString();
    }
    
    private static float ConvertPercents(string percents)
    {
        try
        {
            percents = percents.TrimEnd('%', ' ');
            var num = float.Parse(percents, CultureInfo.InvariantCulture) / 100f;
            return num;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static string ConvertDice(string dice)
    {
        try
        {
            
            var price = ReplaceWhitespace(dice, string.Empty);

            if (price[0] == 'd' || price[0] == 'к')
            {
                price = $"1{price}";
            }

            return price;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static readonly Regex RegexWhitespace = new Regex(@"\s+");
    
    public static string ReplaceWhitespace(string input, string replacement) 
    {
        return RegexWhitespace.Replace(input, replacement);
    }
}