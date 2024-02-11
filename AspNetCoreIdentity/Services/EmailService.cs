using AspNetCoreIdentity.Configurations;
using AspNetCoreIdentity.CustomMailMessages;
using System.Net.Mail;

namespace AspNetCoreIdentity.Services
{
    public class EmailService : IEmailService
    {

        private readonly IEmailServiceSettings _settings;
        private readonly SmtpClient _smtpClient;

        public EmailService(IEmailServiceSettings settings, SmtpClient smtpClient)
        {
            _settings = settings;
            _smtpClient = smtpClient;
        }

        public async Task SendResetPasswordEmail(string link, string toEmail)
        {
            await _smtpClient.SendMailAsync(
                MailMessageFactory.CreateResetPasswordMailMessage(_settings.SenderMail,toEmail,link)
            );
        }

    }
}
