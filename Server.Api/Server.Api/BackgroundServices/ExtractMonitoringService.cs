using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GovServices.Server.Interfaces;

namespace GovServices.Server.BackgroundServices;

public class ExtractMonitoringService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExtractMonitoringService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public ExtractMonitoringService(IServiceProvider serviceProvider, ILogger<ExtractMonitoringService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Проверка выписок ЗАГС и Росреестр запущена в {time}", DateTimeOffset.Now);
            using var scope = _serviceProvider.CreateScope();
            var checker = scope.ServiceProvider.GetRequiredService<IExtractChecker>();
            await checker.CheckAllPendingRequestsAsync();
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
