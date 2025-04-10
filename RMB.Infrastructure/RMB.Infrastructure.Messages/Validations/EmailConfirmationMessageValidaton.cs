using FluentValidation;
using RMB.Abstractions.Infrastructure.Messages;


namespace RMB.Infrastructure.Messages.Validations
{
    public class EmailConfirmationMessageValidator : AbstractValidator<EmailConfirmationMessage>
    {
        public EmailConfirmationMessageValidator()
        {
            RuleFor(x => x.ToEmail)
                .NotEmpty().WithMessage("O campo 'ToEmail' é obrigatório.")
                .EmailAddress().WithMessage("O campo 'ToEmail' precisa ser um e-mail válido.")
                .MaximumLength(254).WithMessage("O e-mail não pode ultrapassar 254 caracteres.")
                .Must(email => !string.IsNullOrWhiteSpace(email)).WithMessage("O campo 'ToEmail' não pode conter apenas espaços em branco.")
                .Must(email => !email.Any(char.IsControl)).WithMessage("O e-mail não pode conter caracteres de controle.")
                .Matches(@"^[^<>]*$").WithMessage("O e-mail não pode conter tags HTML.")
                .Must(email => !email.Contains('<') && !email.Contains('>')).WithMessage("O e-mail não pode conter caracteres HTML como '<' ou '>'.");


            RuleFor(x => x.ConfirmationLink)
                .Must(link => Uri.IsWellFormedUriString(link, UriKind.Absolute))
                .WithMessage("O link de confirmação precisa ser uma URL válida.")
                .NotEmpty().WithMessage("O campo 'ConfirmationLink' é obrigatório.");
        }
    }
}
