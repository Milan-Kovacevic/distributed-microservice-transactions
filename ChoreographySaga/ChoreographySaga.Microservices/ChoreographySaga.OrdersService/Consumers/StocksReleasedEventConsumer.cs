using ChoreographySaga.Contracts.Events;
using ChoreographySaga.Contracts.Responses;
using ChoreographySaga.OrdersService.Abstractions;
using MassTransit;

namespace ChoreographySaga.OrdersService.Consumers;

public class StocksReleasedEventConsumer(IOrderService orderService, ILogger<StocksReleasedEventConsumer> logger) : IConsumer<StocksReleasedEvent>
{
    public async Task Consume(ConsumeContext<StocksReleasedEvent> context)
    {
        // Stock reserve failed, so we can need to update the status of the order to rejected
        // DISTRIBUTED TRANSACTION FAILED - RUNNING COMPENSATION LOCAL TRANSACTION
        await orderService.UpdateOrderStatus(context.Message.OrderUuid, OrderStatus.Rejected);
        
        logger.LogInformation(
            "Distributed Order Transaction[uuid={OrderUuid}] failed, reason={Reason}",
            context.Message.OrderUuid, context.Message.Reason);
        
        // TODO: Notify the client that sent the initial order request via ex. WebSocket or SSE...
    }
}