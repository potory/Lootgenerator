using System.ComponentModel.DataAnnotations;

namespace LootGenerator.Contracts.Requests.Collections;

public class PostCollectionRequest
{
    [Display(Name = "Название")]
    [Required(ErrorMessage = "Поле \"Название\" является обязательным")]
    public string Name { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
}