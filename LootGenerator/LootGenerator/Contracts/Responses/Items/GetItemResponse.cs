using System.ComponentModel.DataAnnotations;

namespace LootGenerator.Contracts.Responses.Items;

public class GetItemResponse
{
    [Display(Name = "ID")]
    public int Id { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "Ссылка")]
    public string Link { get; set; }
    [Display(Name = "Цена")]
    public string Cost { get; set; }
}