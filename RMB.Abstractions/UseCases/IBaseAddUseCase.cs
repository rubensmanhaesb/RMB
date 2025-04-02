namespace RMB.Abstractions.UseCases
{
    /// <summary>
    /// Represents the application layer contract for adding a new entity using a Data Transfer Object (DTO).
    /// Ensures that implementing classes expose an asynchronous method to perform the creation 
    /// and return a corresponding result DTO.
    /// </summary>
    /// <typeparam name="TDto">The DTO type containing the data required to create the entity.</typeparam>
    /// <typeparam name="TDtoResult">The DTO type representing the result of the creation operation.</typeparam>
    public interface IBaseAddUseCase<TDto, TDtoResult>
    {
        /// <summary>
        /// Asynchronously adds a new entity based on the provided DTO.
        /// </summary>
        /// <param name="dto">The DTO containing the data for entity creation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The result DTO representing the newly created entity.</returns>
        Task<TDtoResult> AddAsync(TDto dto, CancellationToken cancellationToken);
    }
}
