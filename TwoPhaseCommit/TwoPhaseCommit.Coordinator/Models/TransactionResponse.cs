namespace TwoPhaseCommit.Coordinator.Models;

public record TransactionResponse(Guid TransactionId, bool IsSuccessful);