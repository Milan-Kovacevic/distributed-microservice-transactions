namespace ChoreographySaga.Contracts.Events;

public record PaymentProcessedEvent(Guid CustomerUuid, Guid OrderUuid);