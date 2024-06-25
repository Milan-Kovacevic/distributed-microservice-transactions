using ChoreographySaga.Contracts.Events;
using ChoreographySaga.StocksService.Persistence;
using ChoreographySaga.StocksService.Persistence.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.StocksService.Consumers;

public class OrderPlacedEventConsumer(StocksDbContext dbContext, IPublishEndpoint publishEndpoint)
    : IConsumer<OrderPlacedEvent>
{
    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        // Order was placed successfully, we can now reserve the stock quantities
        // and begin the local ACID transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        string? failReason = null;
        
        foreach (var product in context.Message.OrderedProducts)
        {
            var productEntity = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductUuid == product.ProductUuid);
            // Case #1: Product with given uuid was not found in the database...
            if (productEntity is null)
            {
                failReason = $"Product with uuid={product.ProductUuid} was not found";
                break;
            }

            // Case #2: Quantity limit reached of a given product...
            if (productEntity.AvailableQuantity < product.Quantity)
            {
                failReason =
                    $"Product [uuid={product.ProductUuid}] has available quantity={productEntity.AvailableQuantity}, but needs to reserve '{product.Quantity}'";
                break;
            }

            productEntity.AvailableQuantity -= product.Quantity;
            // Creating a product reservation history entry (will be used for the purposes of saga compensation logic) 
            var reservedProduct = new ReservedProduct()
            {
                ReservationUuid = Guid.NewGuid(),
                ProductUuid = productEntity.ProductUuid,
                Quantity = product.Quantity,
                OrderUuid = context.Message.OrderUuid
            };
            dbContext.ReservedProducts.Add(reservedProduct);
            await dbContext.SaveChangesAsync();
        }

        if (failReason is not null)
        {
            // Publish event for stock released event
            // (local acid transaction will not be completed, since commit transaction will not be called)
            await publishEndpoint.Publish(new StocksReleasedEvent(context.Message.OrderUuid, failReason));
            return;
        }

        await transaction.CommitAsync();
        // Publish event for stock reserved event (local transaction succeeded)
        await publishEndpoint.Publish(new StocksReservedEvent(context.Message.CustomerUuid, context.Message.OrderUuid, context.Message.TotalPrice));
    }
}