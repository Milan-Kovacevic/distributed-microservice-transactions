using TransactionalOutbox.Contracts.Requests;
using TransactionalOutbox.Contracts.Responses;

namespace FlightsService.Abstractions;

public interface IFlightService
{
    Task<IEnumerable<FlightDetailsResponse>> GetAllFlights();
    Task<IEnumerable<BookedFlightResponse>> GetAllBookedFlights();
    Task<Guid?> BookAFlight(BookFlightRequest request);
}