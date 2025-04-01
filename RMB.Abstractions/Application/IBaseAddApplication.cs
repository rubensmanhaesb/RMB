using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Represents the application layer contract for adding a new entity using a request model.
    /// Defines a method that accepts a request DTO and returns a result DTO upon completion.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request model used to create the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result returned after the entity is added.</typeparam>
    public interface IBaseAddApplication<TRequest, TResult> : IDisposable
    {
        /// <summary>
        /// Asynchronously adds a new entity based on the given request model.
        /// </summary>
        /// <param name="request">The request DTO containing the data to create the entity.</param>
        /// <returns>The result DTO representing the created entity.</returns>
        Task<TResult> AddAsync(TRequest request);
    }
}
