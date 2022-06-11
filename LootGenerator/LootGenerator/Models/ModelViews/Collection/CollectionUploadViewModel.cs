using System.ComponentModel.DataAnnotations;

namespace LootGenerator.Models.ModelViews.Collection;

public class CollectionUploadViewModel
{
    [Required(ErrorMessage = "Введите имя загружаемой коллекции")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Пожалуйста, выберите файл")]
    public IFormFile File { get; set; }
}