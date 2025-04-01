namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Represents the application layer contract for updating an entity using a request model.
    /// Defines a method that accepts a request DTO and returns a result DTO after the update operation.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request model containing the data to update the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result returned after the entity is updated.</typeparam>
    public interface IBaseUpdateApplication<TRequest, TResult>
    {
        /// <summary>
        /// Asynchronously updates an entity based on the given request model.
        /// </summary>
        /// <param name="request">The request DTO containing the updated data.</param>
        /// <returns>The result DTO representing the updated entity.</returns>
        Task<TResult> UpdateAsync(TRequest request);
    }
}
