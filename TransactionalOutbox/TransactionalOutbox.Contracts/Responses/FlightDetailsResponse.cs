namespace TransactionalOutbox.Contracts.Responses;

public record FlightDetailsResponse(
    Guid FlightUuid,
    string DepartureAirport,
    string ArrivalAirport,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    decimal Price);