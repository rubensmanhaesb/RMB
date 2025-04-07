using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMB.Core.ValuesObjects.Logradouro
{
    /// <summary>
    /// Defines a contract for services that retrieve address information from a CEP (Brazilian postal code).
    /// </summary>
    public interface IViaCepService
    {
        /// <summary>
        /// Asynchronously retrieves address information from a remote source using the provided CEP.
        /// </summary>
        /// <param name="cep">The Brazilian postal code (CEP) in the format "00000-000".</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the request.</param>
        /// <returns>An <see cref="Endereco"/> object with address details, or null if the CEP is not found.</returns>
        Task<Endereco?> GetCepAsync(string cep, CancellationToken cancellationToken = default);
    }
}
