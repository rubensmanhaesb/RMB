namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Defines a contract for the application layer to handle entity updates
    /// using a request model (DTO). Returns a result model after processing the update.
    /// </summary>
    /// <typeparam name="TRequest">The type of the input DTO containing data to update the entity.</typeparam>
    /// <typeparam name="TResult">The type of the output DTO returned after the update operation.</typeparam>
    public interface IBaseUpdateApplication<TRequest, TResult>
    {
        /// <summary>
        /// Asynchronously updates an existing entity using the provided request model.
        /// </summary>
        /// <param name="request">The input DTO with updated values.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the updated result DTO.</returns>
        Task<TResult> UpdateAsync(TRequest request, CancellationToken cancellationToken);
    }
}
