using FluentValidation;

namespace RMB.Core.ValuesObjects.Logradouro.Validations
{
    /// <summary>
    /// Validador FluentValidation para o Value Object Endereco.
    /// Inclui validações de formato e integração com a API ViaCEP para verificar se o endereço corresponde ao CEP informado.
    /// </summary>
    public class EnderecoValidation : AbstractValidator<Endereco>
    {
        private readonly IViaCepService _viaCepService;

        /// <summary>
        /// Construtor utilizado para validações básicas sem injeção de dependência.
        /// </summary>
        public EnderecoValidation()
        {
            _viaCepService = new ViaCepService();

            // Regras de validação
            ValidateCep();
            ValidateNumero();
            ValidateComplemento();
            ValidateEnderecoIbge();
        }

        /// <summary>
        /// Valida o formato do CEP.
        /// </summary>
        private void ValidateCep()
            => RuleFor(e => e.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 00000-000.");

        /// <summary>
        /// Valida o campo 'Número' (número do imóvel).
        /// </summary>
        private void ValidateNumero()
            => RuleFor(e => e.Numero)
                .NotEmpty().WithMessage("O número é obrigatório.")
                .MaximumLength(20).WithMessage("O número deve ter no máximo {MaxLength} caracteres.");

        /// <summary>
        /// Valida o campo 'Complemento'.
        /// </summary>
        private void ValidateComplemento()
            => RuleFor(e => e.Complemento)
                .NotEmpty().WithMessage("O complemento é obrigatório.")
                .MaximumLength(100).WithMessage("O complemento deve ter no máximo {MaxLength} caracteres.");

        /// <summary>
        /// Valida se as informações de endereço (logradouro, bairro, cidade, UF) conferem com os dados do ViaCEP.
        /// </summary>
        private void ValidateEnderecoIbge()
        {
            RuleFor(e => e)
                .CustomAsync(async (endereco, context, cancellation) =>
                {
                    var cepData = await _viaCepService.GetCepAsync(endereco.Cep, cancellation);
                    if (cepData == null)
                    {
                        context.AddFailure("Cep", "CEP não encontrado no ViaCEP.");
                        return;
                    }

                    if (!string.Equals(endereco.Logradouro?.Trim(), cepData.Logradouro?.Trim(), StringComparison.OrdinalIgnoreCase))
                        context.AddFailure("Logradouro", "O logradouro não confere com o CEP informado.");

                    if (!string.Equals(endereco.Localidade?.Trim(), cepData.Localidade?.Trim(), StringComparison.OrdinalIgnoreCase))
                        context.AddFailure("Localidade", "A cidade não confere com o CEP informado.");

                    if (!string.Equals(endereco.Uf?.Trim(), cepData.Uf?.Trim(), StringComparison.OrdinalIgnoreCase))
                        context.AddFailure("Uf", "A UF (estado) não confere com o CEP informado.");

                    if (!string.Equals(endereco.Bairro?.Trim(), cepData.Bairro?.Trim(), StringComparison.OrdinalIgnoreCase))
                        context.AddFailure("Bairro", "O bairro não confere com o CEP informado.");
                });
        }
    }
}
