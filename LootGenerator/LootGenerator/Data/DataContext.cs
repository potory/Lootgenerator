using LootGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace LootGenerator.Data;

public class DataContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemCollection> Collections { get; set; }
    public DbSet<ItemCollectionRecord> CollectionRecords { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}