using FluentValidation;
using RMB.Abstractions.Infrastructure.Messages.Entities;


namespace RMB.Infrastructure.Messages.Validations
{
    /// <summary>
    /// Validates EmailConfirmationMessage objects using FluentValidation rules.
    /// </summary>
    /// <remarks>
    /// This validator ensures:
    /// - Required fields are present
    /// - Email format is valid
    /// - No potentially dangerous content
    /// - Field length constraints
    /// - Business rule compliance
    /// </remarks>
    public class EmailConfirmationMessageValidator : AbstractValidator<EmailConfirmationMessage>
    {
        /// <summary>
        /// Initializes the validation rules for the EmailConfirmationMessage object.
        /// </summary>
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

            RuleFor(x => x.Id)
                .NotNull().WithMessage("O campo 'UserId' é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O campo 'UserId' não pode ser um GUID vazio.");
        }
    }
}
