using ChoreographySaga.Contracts.Requests;
using ChoreographySaga.Contracts.Responses;
using ChoreographySaga.OrdersService.Persistence.Entities;

namespace ChoreographySaga.OrdersService.Abstractions;

public interface IOrderService
{
    Task<OrderPlacedResponse> PlaceNewOrder(PlaceOrderRequest request, CancellationToken token = default);
    Task<bool> UpdateOrderStatus(Guid orderUuid, OrderStatus status, CancellationToken token = default);
    Task<OrderDetailsResponse?> GetOrderDetails(Guid orderUuid, CancellationToken token = default);
}