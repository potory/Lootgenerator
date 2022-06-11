using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LootGenerator.Validation;

namespace LootGenerator.Models;

public class ItemCollection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "ID")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(128)]
    [DataType(DataType.Text)]
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    [MaxLength(512)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание")]
    public string Description { get; set; }
    
    public List<ItemCollectionRecord> Records { get; set; }
}

public class ItemCollectionRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordId { get; set; }
    [Required]
    public float Chance { get; set; }
    [Required]
    [MaxLength(32)]
    [DiceRoll]
    public string Count { get; set; }
    
    [Required]
    public int CollectionId { get; set; }
    [Required]
    public int ItemId { get; set; }

    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; }
    
    [ForeignKey(nameof(CollectionId))]
    public ItemCollection Collection { get; set; }
}