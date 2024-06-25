using System.ComponentModel.DataAnnotations;
using ChoreographySaga.Contracts.Responses;

namespace ChoreographySaga.OrdersService.Persistence.Entities;

public class Order
{
    [Key] public Guid OrderUuid { get; init; }
    public required DateTime OrderDate { get; init; }
    public required OrderStatus Status { get; set; }
    public required decimal TotalPrice { get; init; }
    public required Guid CustomerUuid { get; init; }
    public virtual IEnumerable<OrderProduct> Products { get; set; } = default!;
}