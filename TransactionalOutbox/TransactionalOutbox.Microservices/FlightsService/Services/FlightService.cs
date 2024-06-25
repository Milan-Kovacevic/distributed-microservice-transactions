using FlightsService.Abstractions;
using FlightsService.Persistence;
using FlightsService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TransactionalOutbox.Contracts.Requests;
using TransactionalOutbox.Contracts.Responses;

namespace FlightsService.Services;

public class FlightService(FlightsDbContext dbContext) : IFlightService
{
    public async Task<IEnumerable<FlightDetailsResponse>> GetAllFlights()
    {
        return await dbContext.Flights.Select(x =>
                new FlightDetailsResponse(x.FlightUuid, x.DepartureAirport, x.ArrivalAirport, x.DepartureTime,
                    x.ArrivalTime, x.Price))
            .ToListAsync();
    }

    public async Task<IEnumerable<BookedFlightResponse>> GetAllBookedFlights()
    {
        return await dbContext.BookedFlights.Select(x =>
                new BookedFlightResponse(x.BookingUuid, x.FlightUuid, x.NumberOfTickets, x.TotalPrice))
            .ToListAsync();
    }

    public async Task<Guid?> BookAFlight(BookFlightRequest request)
    {
        var flight = await dbContext.Flights.FirstOrDefaultAsync(x => x.FlightUuid == request.FlightUuid);
        if (flight is null) return null;

        // Starting the local acid database transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var flightReservation = new BookedFlight()
        {
            BookingUuid = Guid.NewGuid(),
            Flight = flight,
            FlightUuid = flight.FlightUuid,
            TotalPrice = flight.Price * request.NumberOfTickets,
            NumberOfTickets = request.NumberOfTickets
        };
        // Serialize the outbox message as json and preserve its type,
        // so it is easier to reconstruct message in the background job and publish it to queue
        var contentJson = JsonConvert.SerializeObject(flightReservation, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        var outboxMessage = new OutboxMessage()
        {
            OutboxUuid = Guid.NewGuid(),
            Type = OutboxType.FlightBooked,
            OccuredOn = DateTime.UtcNow,
            ContentJson = contentJson
        };
        // Both insert statements in the database will be performed as one acid transaction
        // (both inserts will be rerolled if error happens during the transaction)
        dbContext.BookedFlights.Add(flightReservation);
        dbContext.OutboxMessages.Add(outboxMessage);
        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return flightReservation.BookingUuid;
    }
}