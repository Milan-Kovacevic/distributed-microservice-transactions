using ChoreographySaga.Contracts.Events;
using ChoreographySaga.Contracts.Requests;
using ChoreographySaga.Contracts.Responses;
using ChoreographySaga.OrdersService.Abstractions;
using ChoreographySaga.OrdersService.Persistence;
using ChoreographySaga.OrdersService.Persistence.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace ChoreographySaga.OrdersService.Services;

public class OrderService(OrdersDbContext dbContext, IPublishEndpoint publishEndpoint) : IOrderService
{
    public async Task<OrderPlacedResponse> PlaceNewOrder(PlaceOrderRequest request, CancellationToken token = default)
    {
        // Begin local ACID transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync(token);
        // ... Skipping validation of request, because we trust clients :)
        var totalPrice = request.Products.Sum(product => product.Quantity * product.UnitPrice);

        var orderProducts = request.Products.Select(x => new OrderProduct()
        {
            OrderProductUuid = Guid.NewGuid(),
            ProductUuid = x.ProductUuid,
            Price = x.UnitPrice,
            Quantity = x.Quantity
        }).ToList();
        
        // Creating new order
        var newOrder = new Order
        {
            OrderUuid = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            CustomerUuid = request.UserUuid,
            TotalPrice = totalPrice,
            Products = orderProducts
        };

        dbContext.Set<Order>().Add(newOrder);
        await dbContext.SaveChangesAsync(token);
        // Completing local transaction
        await transaction.CommitAsync(token);
        // Publishing an event to MessageQueue
        await publishEndpoint.Publish(
            new OrderPlacedEvent(newOrder.CustomerUuid, newOrder.OrderUuid, newOrder.TotalPrice, 
                orderProducts.Select(x=> new OrderProductDto(x.ProductUuid, x.Quantity))), token);

        var response = new OrderPlacedResponse(newOrder.OrderUuid);
        return response;
    }

    public async Task<bool> UpdateOrderStatus(Guid orderUuid, OrderStatus status, CancellationToken token = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(token);
        var order = await dbContext.Set<Order>().FirstOrDefaultAsync(x => x.OrderUuid == orderUuid, token);
        if (order is null) return false;

        order.Status = status;
        await dbContext.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        return true;
    }

    public async Task<OrderDetailsResponse?> GetOrderDetails(Guid orderUuid, CancellationToken token = default)
    {
        var order = await dbContext
            .Set<Order>()
            .Include(o => o.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OrderUuid == orderUuid, token);

        if (order is null) return null;
        var response = new OrderDetailsResponse(order.OrderUuid, order.Status.GetDisplayName(), order.TotalPrice, order.OrderDate,
            order.Products.Select(x => new OrderProductResponse(x.ProductUuid, x.Quantity, x.Price)).ToList());
        return response;
    }
}