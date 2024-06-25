using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TwoPhaseCommit.Coordinator.Models;
using TwoPhaseCommit.Shared.Requests;

namespace TwoPhaseCommit.Coordinator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoordinatorController(
    ILogger<CoordinatorController> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<TransactionConfig> options)
    : ControllerBase
{
    private readonly List<string> _participants = options.Value.Participants;

    [HttpPost("Begin")]
    public IActionResult BeginTransaction([FromBody] CoordinatorRequest request)
    {
        var transactionFailed = false;
        var httpClient = httpClientFactory.CreateClient();

        // 1. Generate unique Transaction Id
        var transactionId = GenerateTransactionId();
        logger.LogCritical("Generated TransactionId {Id}", transactionId);
        // 2. Loop through the hosts that are included in a transaction
        // (Initiate the first phase of the 2PC with the prepare message)
        try
        {
            foreach (var participant in _participants)
            {
                // 2.1. Create the transaction request for the participants
                var transactionRequest = new FirstPhaseRequest(transactionId, $"Data-{request.Data}");

                // 2.2. Serialize the request
                var jsonData = new StringContent(JsonSerializer.Serialize(transactionRequest),
                    Encoding.UTF8, "application/json");

                // 2.3 Create the http request object
                var message = new HttpRequestMessage(HttpMethod.Post, $"{participant}/api/Prepare");
                message.Content = jsonData;

                // 2.4 Send the request and wait for the response
                var response = httpClient.Send(message);
                if (response.IsSuccessStatusCode) continue;

                transactionFailed = true;
                break;
            }

            logger.LogCritical("{Id}: Completed prepare phase, transaction status: {failed} (failed)",
                transactionId, transactionFailed);
        }
        catch (Exception ex)
        {
            transactionFailed = true;
            logger.LogError(ex.Message);
        }


        // 3. If the previous prepare phase was successful (transaction has not failed),
        // proceed to second phase of the 2PC - commit prepared changes
        if (!transactionFailed)
        {
            try
            {
                foreach (var participant in _participants)
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, $"{participant}/api/Commit");
                    var transactionRequest = new SecondPhaseRequest(transactionId);
                    var jsonData = new StringContent(JsonSerializer.Serialize(transactionRequest),
                        Encoding.UTF8, "application/json");
                    message.Content = jsonData;
                    httpClient.Send(message);
                }
            }
            catch(Exception ex)
            {
                // If any request fails in the process, transaction should be rolled out by the coordinator
                transactionFailed = true;
                logger.LogError(ex.Message);
            }

            logger.LogCritical("{Id}: Completed commit phase, transaction status: {failed} (failed)", transactionId,
                transactionFailed);
        }

        
        // 4. If previous steps failed, abort the transaction and re-roll the changes
        if (transactionFailed)
        {
            foreach (var participant in _participants)
            {
                try
                {
                    var message = new HttpRequestMessage(HttpMethod.Post, $"{participant}/api/Abort");
                    var transactionRequest = new SecondPhaseRequest(transactionId);
                    var jsonData = new StringContent(JsonSerializer.Serialize(transactionRequest),
                        Encoding.UTF8, "application/json");
                    message.Content = jsonData;
                    httpClient.Send(message);
                }
                catch(Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }

            logger.LogCritical("{Id}: Completed abort phase", transactionId);
        }


        // Create the transaction response for the caller
        var clientResponse = new TransactionResponse(transactionId, !transactionFailed);

        // Return the http response to the caller
        return Ok(clientResponse);
    }

    private static Guid GenerateTransactionId() => Guid.NewGuid();
}