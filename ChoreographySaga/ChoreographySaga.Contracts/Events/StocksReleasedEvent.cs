namespace ChoreographySaga.Contracts.Events;

public record StocksReleasedEvent(Guid OrderUuid, string Reason);