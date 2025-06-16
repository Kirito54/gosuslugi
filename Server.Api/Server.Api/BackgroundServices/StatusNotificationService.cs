using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using GovServices.Server.Data;
using GovServices.Server.Interfaces;
using Npgsql;

namespace GovServices.Server.BackgroundServices;

public class StatusNotificationService : BackgroundService
{
    private readonly IServiceProvider _services;

    public StatusNotificationService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var lastCheck = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var email = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var logs = await ctx.ApplicationLogs
                    .Where(l => l.Timestamp > lastCheck && l.Action.StartsWith("Updated:"))
                    .Include(l => l.Application)
                    .ToListAsync(stoppingToken);

                foreach (var log in logs)
                {
                    var app = await ctx.Applications
                        .Include(a => a.AssignedTo)
                        .FirstOrDefaultAsync(a => a.Id == log.ApplicationId, stoppingToken);

                    if (app?.AssignedTo != null)
                    {
                        await email.SendEmailAsync(
                            app.AssignedTo.Email!,
                            "Статус заявки изменён",
                            $"Вашей заявке {app.Number} назначен новый статус: {app.Status}");
                    }
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "42P01")
            {
                // Table does not exist yet. Skip iteration.
            }

            lastCheck = DateTime.UtcNow;
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
