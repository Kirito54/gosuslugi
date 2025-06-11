using GovServices.Server.Entities;
using System.Threading.Tasks;

namespace GovServices.Server.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
    Task SendPasswordReminderAsync(ApplicationUser user);
}
