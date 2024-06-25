namespace TwoPhaseCommit.Shared.Requests;

// Models the request that the 2pc coordinator sends in the second phase - commit or abort phase
public record SecondPhaseRequest(Guid TransactionId);