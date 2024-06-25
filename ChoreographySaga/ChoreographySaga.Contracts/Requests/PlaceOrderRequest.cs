namespace ChoreographySaga.Contracts.Requests;

public record PlaceOrderRequest(Guid UserUuid, List<OrderItemRequest> Products);

public record OrderItemRequest(Guid ProductUuid, decimal Quantity, decimal UnitPrice);