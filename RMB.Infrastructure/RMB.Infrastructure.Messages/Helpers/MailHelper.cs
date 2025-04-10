using Microsoft.Extensions.Configuration;
using RMB.Abstractions.Infrastructure.Messages;
using System.Net.Mail;

namespace RMB.Infrastructure.Messages.Helpers
{
    /// <summary>
    /// Helper class for sending email confirmation messages.
    /// </summary>
    public class MailHelper
    {
        private readonly string? _host;
        private readonly int? _port;
        private readonly string? _emailFrom;
        private readonly string? _emailTo;

        /// <summary>
        /// Initializes the MailHelper with SMTP configuration.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        public MailHelper(IConfiguration configuration)
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
            var subject = "Confirmação de Cadastro - JiuJitsu App";
            var body = GenerateBody(emailConfirmationMessage);

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

        /// <summary>
        /// Generates the HTML body for the email confirmation message.
        /// </summary>
        /// <param name="emailConfirmationMessage">The confirmation message object.</param>
        /// <returns>HTML content of the email body.</returns>
        private string GenerateBody(EmailConfirmationMessage emailConfirmationMessage)
        {
            return @$"
                <div>
                    <p>Olá,</p>
                    <p>Você se cadastrou no <strong>JiuJitsu App</strong> e precisamos confirmar o seu endereço de e-mail.</p>
                    <p>Por favor, clique no link abaixo para confirmar seu cadastro:</p>
                    <p>
                        <a href=""{emailConfirmationMessage.ConfirmationLink}"" target=""_blank"">
                            Confirmar meu e-mail
                        </a>
                    </p>
                    <p>Se você não realizou este cadastro, por favor ignore este e-mail.</p>
                    <br/>
                    <p>Equipe JiuJitsu App</p>
                </div>
            ";
        }
    }
}
