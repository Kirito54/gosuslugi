using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Npgsql;

namespace GovServices.Server.BackgroundServices;

public class PasswordReminderService : BackgroundService
{
    private readonly IServiceProvider _services;

    public PasswordReminderService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _services.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var email = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var cutoff = DateTime.UtcNow.AddDays(-30);
                var weekAgo = DateTime.UtcNow.AddDays(-7);

                var users = await ctx.Set<ApplicationUser>()
                    .Where(u => u.PasswordLastChangedAt <= cutoff)
                    .ToListAsync(stoppingToken);

                foreach (var u in users)
                {
                    var lastRem = await ctx.PasswordChangeLogs
                        .Where(l => l.UserId == u.Id && l.Type == "ReminderSent")
                        .OrderByDescending(l => l.Timestamp)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (lastRem == null || lastRem.Timestamp <= weekAgo)
                    {
                        await email.SendPasswordReminderAsync(u);
                        ctx.PasswordChangeLogs.Add(new PasswordChangeLog
                        {
                            UserId = u.Id,
                            Type = "ReminderSent",
                            Timestamp = DateTime.UtcNow
                        });
                    }
                }

                await ctx.SaveChangesAsync(stoppingToken);
            }
            catch (PostgresException ex) when (ex.SqlState == "42P01")
            {
                // Table does not exist yet. Skip iteration.
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
