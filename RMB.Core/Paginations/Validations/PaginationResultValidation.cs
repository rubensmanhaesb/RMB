using FluentValidation;
using RMB.Abstractions.Shared.Contracts.Paginations.Responses;

namespace RMB.Core.Paginations.Validations
{
    public class PaginationResultValidation<T> : AbstractValidator<PaginatedResult<T>>
    {
        public PaginationResultValidation()
        {
            ConfigureRules();
        }

        private void ConfigureRules()
        {
            RuleFor(x => x.Pagination.PageNumber)
                .Must((result, pageNumber) => pageNumber <= result.Pagination.TotalPages)
                .WithMessage("O número da página solicitada excede o total de páginas disponíveis.");

        }
    }
}