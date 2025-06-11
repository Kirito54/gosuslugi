using GovServices.Server.Interfaces;

namespace GovServices.Server.Services;

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        // Placeholder implementation
        return Task.CompletedTask;
    }
}
