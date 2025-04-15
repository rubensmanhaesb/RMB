using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Defines the application layer contract for deleting an entity using a request model.
    /// Provides a method that accepts a request DTO and returns a result DTO upon successful deletion.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request model containing the data required to perform the deletion.</typeparam>
    /// <typeparam name="TResult">The type of the result returned after the entity is deleted.</typeparam>
    public interface IBaseDeleteApplication<TRequest, TResult> 
    {
        /// <summary>
        /// Asynchronously deletes an entity based on the provided request model.
        /// </summary>
        /// <param name="request">The request DTO used to identify the entity to be deleted.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The result DTO representing the outcome of the delete operation.</returns>
        Task<TResult> DeleteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
