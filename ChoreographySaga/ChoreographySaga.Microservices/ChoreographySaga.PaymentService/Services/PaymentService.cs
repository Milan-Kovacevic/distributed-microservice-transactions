using ChoreographySaga.Contracts.Responses;
using ChoreographySaga.PaymentService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.PaymentService.Services;

public class PaymentService(PaymentDbContext dbContext) : IPaymentService
{
    public async Task<PaymentClientDetailsResponse?> GetClientDetails(Guid userUuid,
        CancellationToken cancellationToken = default)
    {
        var client = await dbContext.Clients.FirstOrDefaultAsync(x => x.UserUuid == userUuid, cancellationToken);
        if (client is null) return null;

        return new PaymentClientDetailsResponse(client.UserUuid, client.Balance, client.FirstName, client.LastName);
    }
}