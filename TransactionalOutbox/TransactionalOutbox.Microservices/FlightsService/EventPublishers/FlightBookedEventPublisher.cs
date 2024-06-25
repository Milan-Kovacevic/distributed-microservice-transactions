using FlightsService.Abstractions;
using FlightsService.Persistence.Entities;
using MassTransit;
using TransactionalOutbox.Contracts.Events;

namespace FlightsService.EventPublishers;

public class FlightBookedEventPublisher(IPublishEndpoint publishEndpoint, ILogger<FlightBookedEventPublisher> logger)
    : IEventPublisher
{
    public async Task PublishEvent(OutboxEntity request)
    {
        if (request is BookedFlight bookedFlight)
        {
            var flightBookedEvent = new FlightBookedEvent(bookedFlight.BookingUuid, bookedFlight.FlightUuid,
                bookedFlight.NumberOfTickets, bookedFlight.TotalPrice);
            await publishEndpoint.Publish(flightBookedEvent);
        }
        else
        {
            logger.LogInformation("Outbox entity is not of correct type, expected {ExpectedType}, but got {Type}",
                typeof(BookedFlight).FullName, typeof(OutboxEntity).FullName);
        }
    }
}