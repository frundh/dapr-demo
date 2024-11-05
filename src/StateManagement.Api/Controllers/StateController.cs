using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dapr.Client;

namespace StateManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StateController : ControllerBase
{
    private readonly ILogger<StateController> _logger;
    private readonly DaprClient _daprClient;

    private string _storeName = "statestore";

    public StateController(ILogger<StateController> logger, IConfiguration configuration, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
        _storeName = configuration["StateStoreName"] ?? _storeName;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken, int updates = 0, bool delete = true, int delay = 0)
    {
        var stateObject = new StateObject();

        await _daprClient.SaveStateAsync<StateObject>(_storeName, stateObject.Id, stateObject, cancellationToken: cancellationToken);
        _logger.LogInformation($"Saved state which created a new entry with an initial etag");

        foreach (var i in Enumerable.Range(0, updates))
        {
            var (sb, eTag) = await _daprClient.GetStateAndETagAsync<StateObject>(_storeName, stateObject.Id, cancellationToken: cancellationToken);
            _logger.LogInformation($"Retrieved state with id '{stateObject.Id}' and etag '{eTag}'");
            
            sb.AddEvent();

            await _daprClient.TrySaveStateAsync<StateObject>(_storeName, stateObject.Id, sb, eTag, cancellationToken: cancellationToken);
            _logger.LogInformation($"Saved state with new etag");

            if (delay > 0)
            {
                _logger.LogInformation($"Delaying for {delay} milliseconds");
                await Task.Delay(delay);
            }
        }

        if (delete)
        {
            var (sb, eTag) = await _daprClient.GetStateAndETagAsync<StateObject>(_storeName, stateObject.Id, cancellationToken: cancellationToken);
            _logger.LogInformation($"Retrieved state with id '{stateObject.Id}' and etag '{eTag}'");

            var isDeleteStateSuccess = await _daprClient.TryDeleteStateAsync(_storeName, stateObject.Id, eTag, cancellationToken: cancellationToken);
            _logger.LogInformation($"Deleted state with id '{stateObject.Id}' and etag: '{eTag}' successfully? '{isDeleteStateSuccess}'");
        }

        return Ok();
    }
}
