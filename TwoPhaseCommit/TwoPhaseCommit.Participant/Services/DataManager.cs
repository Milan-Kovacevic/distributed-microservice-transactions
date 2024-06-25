using TwoPhaseCommit.Participant.Abstractions;
using TwoPhaseCommit.Participant.Persistence;
using TwoPhaseCommit.Participant.Persistence.Entities;

namespace TwoPhaseCommit.Participant.Services;

public class DataManager(ILogger<DataManager> logger, AppDbContext dbContext) : IDataManager
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public async Task LockTable(CancellationToken cancellationToken = default)
    {
        await Semaphore.WaitAsync(cancellationToken);
    }

    public async Task PrepareData(TransactionLog transactionLog, CancellationToken cancellationToken = default)
    {
        dbContext.Logs.Add(transactionLog);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitData(TransactionEntity transaction, TransactionLog transactionLog,
        CancellationToken cancellationToken = default)
    {
        transactionLog.Status = TransactionStatus.Commited;
        dbContext.Transactions.Add(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public void UnlockTable()
    {
        if (Semaphore.CurrentCount == 0)
            Semaphore.Release();
    }
}