namespace RMB.Abstractions.UseCases
{
    /// <summary>
    /// Defines the contract for adding entities in the application layer.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to add a new entity using a Data Transfer Object (DTO) and return a result DTO.
    /// </summary>
    /// <typeparam name="TDto">The DTO type containing the data for entity creation.</typeparam>
    /// <typeparam name="TDtoResult">The DTO type representing the result of the operation.</typeparam>
    public interface IBaseAddUseCase<TDto, TDtoResult>
    {
        /// <summary>
        /// Adds a new entity asynchronously using a Data Transfer Object (DTO).
        /// </summary>
        /// <param name="dto">The DTO containing the data for entity creation.</param>
        /// <returns>A task that resolves to the result DTO representing the created entity.</returns>
        Task<TDtoResult> AddAsync(TDto dto);
    }
}
