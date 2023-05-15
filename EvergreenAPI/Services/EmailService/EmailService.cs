using EvergreenAPI.DTO;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;

namespace EvergreenAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailDto _emailDto;
        private readonly ILogger<EmailService> _logger;



        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public EmailService (IOptions<EmailDto> emailDto, ILogger<EmailService> logger)
        {
           _emailDto = emailDto.Value;
            this._logger = logger;
            this._logger.LogInformation("Create SendMailService");
        }


        public class MailContent
        {
            public string To { get; set; }              // Địa chỉ gửi đến
            public string Subject { get; set; }         // Chủ đề (tiêu đề email)
            public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email
        }


        // Gửi email, theo nội dung trong mailContent
        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_emailDto.DisplayName, _emailDto.Mail);
            email.From.Add(new MailboxAddress(_emailDto.DisplayName, _emailDto.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder
            {
                HtmlBody = mailContent.Body
            };
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new SmtpClient();

            try
            {
                smtp.Connect(_emailDto.Host, _emailDto.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailDto.Mail, _emailDto.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            _logger.LogInformation("send mail to " + mailContent.To);
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }

       
    }
}
