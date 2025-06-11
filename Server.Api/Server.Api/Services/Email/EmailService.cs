using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace GovServices.Server.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var settings = _configuration.GetSection("EmailSettings");
        var fromAddress = settings["FromAddress"] ?? string.Empty;
        var host = settings["SmtpHost"] ?? string.Empty;
        var port = settings.GetValue<int>("SmtpPort");
        var user = settings["SmtpUser"] ?? string.Empty;
        var pass = settings["SmtpPass"] ?? string.Empty;

        var msg = new MimeMessage();
        msg.From.Add(MailboxAddress.Parse(fromAddress));
        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(user, pass);
        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }

    public async Task SendPasswordReminderAsync(ApplicationUser user)
    {
        var subject = "Напоминание: смена пароля";
        var body = $"Уважаемый {user.FullName}, пожалуйста, смените пароль по ссылке.";
        await SendEmailAsync(user.Email!, subject, body);
    }
}
