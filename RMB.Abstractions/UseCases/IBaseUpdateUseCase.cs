namespace RMB.Abstractions.UseCases
{
    /// <summary>
    /// Represents the application layer contract for updating an entity using a Data Transfer Object (DTO).
    /// Ensures that implementing classes expose an asynchronous method to perform the update operation 
    /// and return a corresponding result DTO.
    /// </summary>
    /// <typeparam name="TDto">The DTO type containing the data to update the entity.</typeparam>
    /// <typeparam name="TDtoResult">The DTO type representing the result of the update operation.</typeparam>
    public interface IBaseUpdateUseCase<TDto, TDtoResult>
    {
        /// <summary>
        /// Asynchronously updates an existing entity based on the provided DTO.
        /// </summary>
        /// <param name="dto">The DTO containing the update data.</param>
        /// <param name="cancellationToken">Token to cancel the update operation if needed.</param>
        /// <returns>The result DTO representing the updated entity.</returns>
        Task<TDtoResult> UpdateAsync(TDto dto, CancellationToken cancellationToken);
    }
}
