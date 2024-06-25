using ChoreographySaga.Contracts.Responses;

namespace ChoreographySaga.PaymentService.Services;

public interface IPaymentService
{
    Task<PaymentClientDetailsResponse?> GetClientDetails(Guid userUuid, CancellationToken cancellationToken = default);
}