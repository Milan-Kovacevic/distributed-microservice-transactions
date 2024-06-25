using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwoPhaseCommit.Participant.Abstractions;
using TwoPhaseCommit.Participant.Persistence;
using TwoPhaseCommit.Participant.Persistence.Entities;
using TwoPhaseCommit.Shared.Requests;

namespace TwoPhaseCommit.Participant.Controllers;

[ApiController]
[Route("api")]
public class ParticipantController(
    ILogger<ParticipantController> logger,
    IDataManager dataManager,
    AppDbContext context) : ControllerBase
{
    [HttpGet("Transactions")]
    public async Task<IActionResult> GetAllTransaction()
    {
        return Ok(await context.Transactions.ToListAsync());
    }

    [HttpGet("Logs")]
    public async Task<IActionResult> GetAllTransactionLogs()
    {
        return Ok(await context.Logs.ToListAsync());
    }

    [HttpPost("Prepare")]
    public async Task<IActionResult> PrepareForCommit([FromBody] FirstPhaseRequest request)
    {
        logger.LogInformation("{Id}: Prepare phase started at {Time}", request.TransactionId, DateTime.UtcNow);
        var entity = await context.Transactions.FirstOrDefaultAsync(x => x.Data == request.Data);
        if (entity is not null)
            return BadRequest();

        var transactionLog = new TransactionLog
        {
            LogId = Guid.NewGuid(),
            Status = TransactionStatus.Prepared,
            Data = request.Data,
            TransactionId = request.TransactionId,
        };
        
        await dataManager.LockTable();
        logger.LogInformation("{Id}: Locked table data", request.TransactionId);
        await Task.Delay(5000);
        await dataManager.PrepareData(transactionLog);
        logger.LogInformation("{Id}: Saved transaction log", request.TransactionId);
        logger.LogInformation("{Id}: Prepare phase finished at {Time}", request.TransactionId, DateTime.UtcNow);
        return Ok();
    }

    [HttpPost("Commit")]
    public async Task<IActionResult> CommitTransaction([FromBody] SecondPhaseRequest request)
    {
        logger.LogInformation("{Id}: Commit phase started at {Time}", request.TransactionId, DateTime.UtcNow);
        var transactionLog = await context.Logs.FirstOrDefaultAsync(x => x.TransactionId == request.TransactionId);
        if (transactionLog is null || transactionLog.Status != TransactionStatus.Prepared)
            return BadRequest();

        var transaction = new TransactionEntity
        {
            Uuid = Guid.NewGuid(),
            Data = transactionLog.Data,
            TransactionId = request.TransactionId
        };
        
        await Task.Delay(5000);
        await dataManager.CommitData(transaction, transactionLog);
        logger.LogInformation("{Id}: Commited transaction data", request.TransactionId);
        dataManager.UnlockTable();
        logger.LogInformation("{Id}: Unlocked table data", request.TransactionId);
        logger.LogInformation("{Id}: Commit phase finished at {Time}", request.TransactionId, DateTime.UtcNow);
        return Ok();
    }

    [HttpPost("Abort")]
    public async Task<IActionResult> AbortTransaction([FromBody] SecondPhaseRequest request)
    {
        var transactionLog = await context.Logs.FirstOrDefaultAsync(x => x.TransactionId == request.TransactionId);
        if (transactionLog is null)
        {
            logger.LogInformation("{Id}: Abort phase finished, no transaction log found", request.TransactionId);
            return Ok();
        }

        if (transactionLog.Status is TransactionStatus.Prepared or TransactionStatus.Commited)
        {
            transactionLog.Status = TransactionStatus.Aborted;
            dataManager.UnlockTable();
            await context.SaveChangesAsync();
            logger.LogInformation("{Id}: Abort phase finished at {Time}", request.TransactionId, DateTime.UtcNow);
        }
        
        // Todo, if state is commited, undo the last commit...
        return Ok();
    }
}