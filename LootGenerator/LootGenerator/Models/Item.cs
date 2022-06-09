using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LootGenerator.Models;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(128)]
    [DataType(DataType.Text)]
    public string Name { get; set; }
    [MaxLength(512)]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
    [MaxLength(256)]
    [DataType(DataType.Url)]
    public string Link { get; set; }
    [MaxLength(32)]
    public string Cost { get; set; }
}