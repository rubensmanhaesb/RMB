using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Defines the application layer contract for adding a new entity using a request model.
    /// Provides a method that accepts a request DTO and returns a result DTO upon successful creation.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request model used to create the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result returned after the entity is created.</typeparam>
    public interface IBaseAddApplication<TRequest, TResult> : IDisposable
    {
        /// <summary>
        /// Asynchronously adds a new entity based on the provided request model.
        /// </summary>
        /// <param name="request">The request DTO containing the data to create the entity.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The result DTO representing the newly created entity.</returns>
        Task<TResult> AddAsync(TRequest request, CancellationToken cancellationToken);
    }
}
