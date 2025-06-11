using Microsoft.Extensions.Hosting;

namespace GovServices.Server.BackgroundServices;

public class StatusNotificationService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // No-op background task
        return Task.CompletedTask;
    }
}
