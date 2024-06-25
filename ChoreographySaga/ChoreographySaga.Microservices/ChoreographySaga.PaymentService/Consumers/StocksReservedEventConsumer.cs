using ChoreographySaga.Contracts.Events;
using ChoreographySaga.PaymentService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.PaymentService.Consumers;

public class StocksReservedEventConsumer(PaymentDbContext dbContext, IPublishEndpoint publishEndpoint)
    : IConsumer<StocksReservedEvent>
{
    public async Task Consume(ConsumeContext<StocksReservedEvent> context)
    {
        // Stocks reservation was successful, we can now process the payment for a given amount
        // and begin the local ACID transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        var client = await dbContext.Clients.FirstOrDefaultAsync(x => x.UserUuid == context.Message.CustomerUuid);
        // Case #1: Client with given uuid was not found in the database...
        if (client is null)
        {
            var failReason = $"Client with uuid={context.Message.CustomerUuid} was not found";
            await SendPaymentFailedEvent(context.Message.CustomerUuid, context.Message.OrderUuid, failReason);
            return;
        }
        
        // Case #2: Client bank balance has insufficient funds...
        if (client.Balance < context.Message.TotalAmount)
        {
            var failReason = $"Client [uuid={context.Message.CustomerUuid}] has insufficient funds";
            await SendPaymentFailedEvent(context.Message.CustomerUuid, context.Message.OrderUuid, failReason);
            return;
        }

        client.Balance -= context.Message.TotalAmount;
        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        
        // Publish event for payment processed event (local transaction succeeded)
        await publishEndpoint.Publish(new PaymentProcessedEvent(context.Message.CustomerUuid, context.Message.OrderUuid));
    }

    private async Task SendPaymentFailedEvent(Guid customerUuid, Guid orderUuid, string reason)
    {
        await publishEndpoint.Publish(new PaymentFailedEvent(customerUuid, orderUuid, reason));
    }
}