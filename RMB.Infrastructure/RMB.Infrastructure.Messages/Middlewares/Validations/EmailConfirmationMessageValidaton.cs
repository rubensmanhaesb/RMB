using FluentValidation;
using RMB.Abstractions.Infrastructure.Messages;


namespace RMB.Infrastructure.Messages.Middlewares.Validations
{
    public class EmailConfirmationMessageValidator : AbstractValidator<EmailConfirmationMessage>
    {
        public EmailConfirmationMessageValidator()
        {
            RuleFor(x => x.ToEmail)
                .NotEmpty().WithMessage("O campo 'ToEmail' é obrigatório.")
                .EmailAddress().WithMessage("O campo 'ToEmail' precisa ser um e-mail válido.");

            RuleFor(x => x.ConfirmationLink)
                .NotEmpty().WithMessage("O campo 'ConfirmationLink' é obrigatório.");
        }
    }
}
