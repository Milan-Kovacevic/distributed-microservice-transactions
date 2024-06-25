using TwoPhaseCommit.Participant.Persistence.Entities;

namespace TwoPhaseCommit.Participant.Abstractions;

public interface IDataManager
{
    Task LockTable(CancellationToken cancellationToken = default);
    Task PrepareData(TransactionLog transactionLog, CancellationToken cancellationToken = default);
    Task CommitData(TransactionEntity entity, TransactionLog transactionLog, CancellationToken cancellationToken = default);
    void UnlockTable();
}