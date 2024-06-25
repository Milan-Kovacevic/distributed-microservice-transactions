namespace ChoreographySaga.Contracts.Events;

public record OrderPlacedEvent(Guid CustomerUuid, Guid OrderUuid, decimal TotalPrice, IEnumerable<OrderProductDto> OrderedProducts);
public record OrderProductDto(Guid ProductUuid, decimal Quantity);