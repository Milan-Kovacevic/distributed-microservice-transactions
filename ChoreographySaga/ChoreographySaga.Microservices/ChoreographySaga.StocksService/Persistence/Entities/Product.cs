using System.ComponentModel.DataAnnotations;

namespace ChoreographySaga.StocksService.Persistence.Entities;

public class Product
{
    [Key]
    public Guid ProductUuid { get; set; }
    public required string Name { get; set; }
    public required decimal UnitPrice { get; set; }
    public required decimal AvailableQuantity { get; set; }
}