using System.ComponentModel.DataAnnotations;

namespace ChoreographySaga.OrdersService.Persistence.Entities;

public class OrderProduct
{
    [Key] public Guid OrderProductUuid { get; init; }
    public Guid ProductUuid { get; init; }
    public required decimal Price { get; init; }
    public required decimal Quantity { get; init; }
}