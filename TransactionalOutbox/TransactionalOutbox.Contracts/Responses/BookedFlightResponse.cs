namespace TransactionalOutbox.Contracts.Responses;

public record BookedFlightResponse(Guid BookingUuid, Guid FlightUuid, int NumberOfTickets, decimal TotalPrice);