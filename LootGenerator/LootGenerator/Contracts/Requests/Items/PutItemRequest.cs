using System.ComponentModel.DataAnnotations;
using LootGenerator.Validation;

namespace LootGenerator.Contracts.Requests.Items;

public class PutItemRequest
{
    [Required]
    public int Id { get; set; }
    [Display(Name = "Название")]
    [Required(ErrorMessage = "Поле \"Название\" является обязательным")]
    public string Name { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "Ссылка")]
    public string Link { get; set; }
    [Display(Name = "Цена")]
    [Required(ErrorMessage = "Поле \"Цена\" является обязательным")]
    [DiceRoll(ErrorMessage = "Неверная формула ролла")]
    public string Cost { get; set; }
}