namespace ChoreographySaga.Contracts.Events;

public record StocksReservedEvent(Guid CustomerUuid, Guid OrderUuid, decimal TotalAmount);