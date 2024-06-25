namespace TransactionalOutbox.Contracts.Requests;

public record BookFlightRequest(Guid FlightUuid, int NumberOfTickets);