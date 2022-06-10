using System.ComponentModel.DataAnnotations;

namespace LootGenerator.Contracts.Requests.Items;

public class PutItemRequest
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    [Required]
    public string Cost { get; set; }
}