using System.ComponentModel.DataAnnotations;

namespace LootGenerator.Contracts.Responses.Collections;

public class RandomizeByCountResponse
{
    public record struct Record
    {
        public string Name { get; }

        public Record(string name)
        {
            Name = name;
        }
    }

    public int Id { get; set; } 
    public string Name { get; set; }
    [Display(Name = "Количество")]
    public int Count { get; set; }
    
    public List<Record> Records { get; set; } = new();
}