using Microsoft.Extensions.Configuration;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using System.Net.Mail;

namespace RMB.Infrastructure.Messages.Services
{
    /// <summary>
    /// Helper class for sending email confirmation messages.
    /// </summary>
    public class MailService
    {
        private readonly string? _host;
        private readonly int? _port;
        private readonly string? _emailFrom;
        private readonly string? _emailTo;

        /// <summary>
        /// Initializes the MailService with SMTP configuration from app settings.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        public MailService(IConfiguration configuration)
        {
            _host = configuration["SmtpSettings:Host"];
            _port = int.Parse(configuration["SmtpSettings:Port"]);
            _emailFrom = configuration["SmtpSettings:EmailFrom"];
            _emailTo = configuration["SmtpSettings:EmailTo"];
        }

        /// <summary>
        /// Sends a confirmation email asynchronously using SMTP.
        /// </summary>
        /// <param name="emailConfirmationMessage">The confirmation message containing the link and user information.</param>
        public async Task SendAsync(EmailConfirmationMessage emailConfirmationMessage)
        {
            var subject = emailConfirmationMessage.Subject;
            var body = emailConfirmationMessage.Body;

            using var smtpClient = new SmtpClient(_host, _port.Value)
            {
                EnableSsl = false
            };

            using var mailMessage = new MailMessage(_emailFrom, _emailTo, subject, body)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
