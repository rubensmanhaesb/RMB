namespace RMB.Abstractions.UseCases
{
    /// <summary>
    /// Represents the application layer contract for deleting an entity using a Data Transfer Object (DTO).
    /// Ensures that implementing classes expose an asynchronous method to perform the deletion 
    /// and return a corresponding result DTO.
    /// </summary>
    /// <typeparam name="TDto">The DTO type containing the data required to delete the entity.</typeparam>
    /// <typeparam name="TDtoResult">The DTO type representing the result of the deletion operation.</typeparam>
    public interface IBaseDeleteUseCase<TDto, TDtoResult>
    {
        /// <summary>
        /// Asynchronously deletes an entity based on the provided DTO.
        /// </summary>
        /// <param name="dto">The DTO containing the data to identify the entity to be deleted.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The result DTO representing the deleted entity.</returns>
        Task<TDtoResult> DeleteAsync(TDto dto, CancellationToken cancellationToken);
    }
}
