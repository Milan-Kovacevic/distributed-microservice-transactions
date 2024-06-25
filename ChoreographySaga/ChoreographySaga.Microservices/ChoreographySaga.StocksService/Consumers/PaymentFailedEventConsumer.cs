using ChoreographySaga.Contracts.Events;
using ChoreographySaga.StocksService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.StocksService.Consumers;

public class PaymentFailedEventConsumer(StocksDbContext dbContext, IPublishEndpoint publishEndpoint)
    : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        // Payment process failed, so we can need to release the reserved stocks for a given order
        // DISTRIBUTED TRANSACTION FAILED - RUNNING COMPENSATION LOCAL TRANSACTION
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        var reservedProducts = dbContext.ReservedProducts
            .Where(x => x.OrderUuid == context.Message.OrderUuid).ToList();
        foreach (var product in reservedProducts)
        {
            var productEntity = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductUuid == product.ProductUuid);
            // Case #1: Product with given uuid was not found in the database
            // (entry was removed from the database, or some logic was run that deleted the item when quantity hit 0, or something else...)
            if (productEntity is null)
            {
                // Scenario #1: Create this missing product entry with the available quantity that need to be compensated, or skip...
                continue;
            }

            // Compensate the reserved quantity of failed order...
            productEntity.AvailableQuantity += product.Quantity;
            await dbContext.SaveChangesAsync();
        }

        await transaction.CommitAsync();
        // Publish the event to let the consumers know that the order transaction failed
        // by passing the reason that this consumer got from the event producer
        await publishEndpoint.Publish(new StocksReleasedEvent(context.Message.OrderUuid, context.Message.Reason));
    }
}