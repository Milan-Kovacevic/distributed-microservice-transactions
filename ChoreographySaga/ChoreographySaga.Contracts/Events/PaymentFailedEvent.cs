namespace ChoreographySaga.Contracts.Events;

public record PaymentFailedEvent(Guid CustomerUuid, Guid OrderUuid, string Reason);