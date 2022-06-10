namespace LootGenerator.Contracts.Responses.Items;

public class GetItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public string Cost { get; set; }
}