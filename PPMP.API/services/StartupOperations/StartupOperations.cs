
namespace Services
{
public class StartupOperationsHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public StartupOperationsHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var operations = scope.ServiceProvider
            .GetRequiredService<IEnumerable<IStartupOperation>>();

        foreach (var operation in operations)
        {
            await operation.ExecuteOperationAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}


}
