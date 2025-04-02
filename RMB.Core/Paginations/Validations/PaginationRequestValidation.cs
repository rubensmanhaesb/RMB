using FluentValidation;
using RMB.Abstractions.Shared.Contracts.Paginations.Requests;

namespace RMB.Core.Paginations.Validations
{
    public class PaginationRequestValidation : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidation()
        {
            ConfigureRules();
        }

        private void ConfigureRules()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("O número da página deveser maior que 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("A quantidade de registro por página precisa estar entre 1 e 100.");
        }
    }
}