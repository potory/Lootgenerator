using System.Globalization;
using System.Text;
using CsvHelper;
using LootGenerator.Models;

namespace LootGenerator.Extensions;

public static class IFormFileExtensions
{
    public static async Task<string> GetString(this IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileBytes = ms.ToArray();
        return Encoding.Default.GetString(fileBytes);
    }

    public static async Task<IEnumerable<T>> GetRecords<T>(this IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        return csv.GetRecords<T>();
    }
}