
namespace RMB.Abstractions.UseCases
{
    /// <summary>
    /// Defines the contract for updating entities in the application layer.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to update an entity using a Data Transfer Object (DTO) and return a result DTO.
    /// </summary>
    /// <typeparam name="TDto">The DTO type containing the data for updating an entity.</typeparam>
    /// <typeparam name="TDtoResult">The DTO type representing the result of the update operation.</typeparam>
    public interface IBaseUpdateUseCase<TDto, TDtoResult>
    {
        /// <summary>
        /// Updates an existing entity asynchronously using a Data Transfer Object (DTO).
        /// </summary>
        /// <param name="dto">The DTO containing the data for the update operation.</param>
        /// <returns>A task that resolves to the result DTO representing the updated entity.</returns>
        Task<TDtoResult> UpdateAsync(TDto dto);
    }
}
