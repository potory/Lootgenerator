using System.ComponentModel.DataAnnotations;

namespace LootGenerator.Contracts.Responses.Collections;

public class RandomizeCollectionResponse
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<ItemRecord> Records { get; } = new List<ItemRecord>();

    public readonly struct ItemRecord
    {
        public int Id { get; }
        [Display(Name = "Название")]
        public string Name { get; }
        [Display(Name = "Цена")]
        public string Price { get; }
        [Display(Name = "Количество")]
        public string Count { get; }

        public ItemRecord(int id, string name, string price, string count)
        {
            Id = id;
            Name = name;
            Price = price;
            Count = count;
        }
    }
}