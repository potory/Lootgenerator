namespace LootGenerator.Contracts.Requests;

public class PostItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public string Cost { get; set; }
}