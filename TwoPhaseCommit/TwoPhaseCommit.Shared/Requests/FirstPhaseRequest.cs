namespace TwoPhaseCommit.Shared.Requests;

// Models the request that the 2pc coordinator sends in the first phase - prepare phase
public record FirstPhaseRequest(Guid TransactionId, string Data);