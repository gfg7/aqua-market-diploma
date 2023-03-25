using AquaServer.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AquaServer.Services.General
{
    public class EmailService : IMessagingService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(string email, string subject, string text, string[] attachments = null)
        {
            MailAddress from = new(_configuration.GetConnectionString("CompanyEmail"));
            MailAddress to = new(email);
            MailMessage msg = new(from, to)
            {
                Subject = subject,
                Body = text,
                IsBodyHtml = true
            };
            if (attachments is not null)
            {
                foreach (var path in attachments)
                {
                    msg.Attachments.Add(new Attachment(path));
                }
            }
            SmtpClient smtp = new("smtp.inbox.ru", 587)
            {
                Credentials = new NetworkCredential(
                _configuration.GetConnectionString("CompanyEmail"),
                _configuration.GetConnectionString("CompanyEmailPassword")
                ),
                EnableSsl = true
            };
            try
            {
                await smtp.SendMailAsync(msg);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка отправки письма-подтверждения. Проверьте доступность своей почты.");
            }
        }
    }
}
