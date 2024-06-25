using System.Text.Json.Serialization;
using MassTransit;
using TransactionalOutbox.Contracts.Events;

namespace PaymentService.Consumers;

public class FlightBookedEventConsumer(ILogger<FlightBookedEventConsumer> logger) : IConsumer<FlightBookedEvent>
{
    public async Task Consume(ConsumeContext<FlightBookedEvent> context)
    {
        // TODO: Process the payment and persist it in the database...
        logger.LogInformation("Consumed {Event}, BookingUuid={BookingUuid}, NumberOfTickets={NumberOfTickets}, TotalPrice={TotalPrice}",
            nameof(FlightBookedEvent), context.Message.BookingUuid, context.Message.NumberOfTickets, context.Message.TotalPrice);
        await Task.CompletedTask;
    }
}