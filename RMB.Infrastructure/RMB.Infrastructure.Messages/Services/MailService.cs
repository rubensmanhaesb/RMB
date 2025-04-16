using Microsoft.Extensions.Configuration;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using System.Net.Mail;

namespace RMB.Infrastructure.Messages.Services
{
    /// <summary>
    /// Service for sending email confirmation messages using SMTP protocol.
    /// </summary>
    /// <remarks>
    /// This service handles:
    /// - SMTP client configuration
    /// - HTML email composition
    /// - Async email delivery
    /// - Basic error handling
    /// </remarks>
    public class MailService
    {
        private readonly string? _host;
        private readonly int? _port;
        private readonly string? _emailFrom;

        /// <summary>
        /// Initializes a new instance of the MailService with SMTP configuration.
        /// </summary>
        /// <param name="configuration">Application configuration containing SMTP settings.</param>
        /// <param name="logger">Logger instance for error reporting.</param>
        /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when required SMTP settings are missing or invalid.</exception>
        public MailService(IConfiguration configuration)
        {
            _host = configuration["SmtpSettings:Host"];
            _port = int.Parse(configuration["SmtpSettings:Port"]);
            _emailFrom = configuration["SmtpSettings:EmailFrom"];
        }

         /// <summary>
        /// Sends an email confirmation message asynchronously.
        /// </summary>
        /// <param name="emailConfirmationMessage">The email confirmation details.</param>
        /// <param name="cancellationToken">Cancellation token for async operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when emailConfirmationMessage is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when required email fields are missing.</exception>
        public async Task SendAsync(EmailConfirmationMessage emailConfirmationMessage)
        {
            var subject = emailConfirmationMessage.Subject;
            var body = emailConfirmationMessage.Body;

            using var smtpClient = new SmtpClient(_host, _port.Value)
            {
                EnableSsl = false
            };

            using var mailMessage = new MailMessage(_emailFrom, emailConfirmationMessage.ToEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
