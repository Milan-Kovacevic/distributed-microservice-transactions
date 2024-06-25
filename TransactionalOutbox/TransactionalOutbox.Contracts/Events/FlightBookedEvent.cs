namespace TransactionalOutbox.Contracts.Events;

public record FlightBookedEvent(Guid BookingUuid, Guid FlightUuid, int NumberOfTickets, decimal TotalPrice);