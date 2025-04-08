using Microsoft.Extensions.Configuration;
using RMB.Abstractions.Infrastructure.Messages;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RMB.Infrastructure.Messages.Helpers
{
    public class MailHelper
    {
        private readonly string? _host;
        private readonly int? _port;
        private readonly string? _emailFrom;
        private readonly string? _emailTo;

        public MailHelper(IConfiguration configuration)
        {
            _host = configuration["SmtpSettings:Host"];
            _port = int.Parse(configuration["SmtpSettings:Port"]);
            _emailFrom = configuration["SmtpSettings:EmailFrom"];
            _emailTo = configuration["SmtpSettings:EmailTo"];
        }

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

        private string GenerateBody(EmailConfirmationMessage emailConfirmationMessage)
        {
            return @$"
                <div>
                    <p>Olá,</p>
                    <p>Você se cadastrou no <strong>JiuJitsu App</strong> e precisamos confirmar seu endereço de e-mail.</p>
                    <p>Por favor, clique no link abaixo para confirmar seu cadastro:</p>
                    <p>
                        <a href=""{emailConfirmationMessage.ConfirmationLink}"" target=""_blank"">
                            Confirmar meu e-mail
                        </a>
                    </p>
                    <p>Se você não realizou este cadastro, ignore este e-mail.</p>
                    <br/>
                    <p>Equipe JiuJitsu App</p>
                </div>
            ";
        }
    }
}
