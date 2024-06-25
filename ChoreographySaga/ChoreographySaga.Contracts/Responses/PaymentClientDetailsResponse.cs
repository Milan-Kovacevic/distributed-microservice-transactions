namespace ChoreographySaga.Contracts.Responses;

public record PaymentClientDetailsResponse(Guid ClientUuid, decimal BankBalance, string FirstName, string LastName);