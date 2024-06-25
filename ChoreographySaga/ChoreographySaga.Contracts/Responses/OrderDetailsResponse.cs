namespace ChoreographySaga.Contracts.Responses;

public record OrderDetailsResponse(Guid OrderUuid, string OrderStatus, decimal TotalPrice, DateTime OrderDate, List<OrderProductResponse> Products);
public record OrderProductResponse(Guid ProductUuid, decimal Quantity, decimal Price);

public enum OrderStatus
{
    Pending,
    Completed,
    Rejected
}