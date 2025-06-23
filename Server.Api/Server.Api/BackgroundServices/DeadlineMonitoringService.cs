using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using System.Text.Json;

namespace GovServices.Server.BackgroundServices;

public class DeadlineMonitoringService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DeadlineMonitoringService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    public DeadlineMonitoringService(IServiceProvider services, ILogger<DeadlineMonitoringService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var email = scope.ServiceProvider.GetService<IEmailService>();

            var services = await ctx.Services.ToListAsync(stoppingToken);
            foreach (var service in services)
            {
                var due = service.ExecutionDeadlineDate ?? service.CreatedAt.AddDays(service.ExecutionDeadlineDays ?? 0);
                var daysLeft = (due - DateTime.UtcNow).TotalDays;

                if (daysLeft < 0 && service.Status != "Просрочено")
                {
                    service.Status = "Просрочено";
                    if (email != null)
                    {
                        await email.SendEmailAsync("admin@example.com", $"Услуга {service.Name} просрочена", $"{service.Name} просрочена");
                    }
                }
                else if (service.Status != "Просрочено")
                {
                    if (daysLeft <= 1)
                    {
                        _logger.LogInformation("Срок услуги {Name} истекает завтра", service.Name);
                    }
                    else if (daysLeft <= 3)
                    {
                        _logger.LogInformation("Срок услуги {Name} истекает через три дня", service.Name);
                    }
                }
            }
            await ctx.SaveChangesAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
