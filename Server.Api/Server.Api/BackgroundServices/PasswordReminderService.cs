using Microsoft.Extensions.Hosting;

namespace GovServices.Server.BackgroundServices;

public class PasswordReminderService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // No-op background task
        return Task.CompletedTask;
    }
}
