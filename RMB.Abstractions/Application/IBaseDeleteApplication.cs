using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Represents the application layer contract for deleting an entity using a request model.
    /// Defines a method that accepts a request DTO and returns a result DTO after the deletion operation.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request model containing the data needed to perform the deletion.</typeparam>
    /// <typeparam name="TResult">The type of the result returned after the entity is deleted.</typeparam>
    public interface IBaseDeleteApplication<TRequest, TResult> : IDisposable
    {
        /// <summary>
        /// Asynchronously deletes an entity based on the given request model.
        /// </summary>
        /// <param name="request">The request DTO containing information to identify the entity to be deleted.</param>
        /// <returns>The result DTO representing the outcome of the deletion.</returns>
        Task<TResult> DeleteAsync(TRequest request);
    }
}
