using System.Threading.Tasks;
using static EvergreenAPI.Services.EmailService.EmailService;

namespace EvergreenAPI.Services.EmailService
{
    public interface IEmailService
    {
        Task SendMail(MailContent mailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
