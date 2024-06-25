using ChoreographySaga.Contracts.Events;
using ChoreographySaga.Contracts.Responses;
using ChoreographySaga.OrdersService.Abstractions;
using MassTransit;

namespace ChoreographySaga.OrdersService.Consumers;

public class PaymentProcessedEventConsumer(IOrderService orderService, ILogger<PaymentProcessedEventConsumer> logger)
    : IConsumer<PaymentProcessedEvent>
{
    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        // Payment was successful, we can update the status of the order
        // ONE SUCCESSFUL DISTRIBUTED TRANSACTION LIFECYCLE ENDS HERE
        await orderService.UpdateOrderStatus(context.Message.OrderUuid, OrderStatus.Completed);

        logger.LogInformation(
            "Distributed Order Transaction[uuid={OrderUuid}] for customer[uuid={CustomerUuid}] completed successfully",
            context.Message.OrderUuid, context.Message.CustomerUuid);

        // TODO: Notify the client that sent the initial order request via ex. WebSocket or SSE...
    }
}