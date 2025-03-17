

namespace RMB.Abstractions.UseCases
{
    /// <summary>
    /// Defines the contract for deleting entities in the application layer.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to delete an entity using a Data Transfer Object (DTO) and return a result DTO.
    /// </summary>
    /// <typeparam name="TDto">The DTO type containing the data for entity deletion.</typeparam>
    /// <typeparam name="TDtoResult">The DTO type representing the result of the operation.</typeparam>
    public interface IBaseDeleteUseCase<TDto, TDtoResult>
    {
        /// <summary>
        /// Deletes an entity asynchronously using a Data Transfer Object (DTO).
        /// </summary>
        /// <param name="dto">The DTO containing the data for entity deletion.</param>
        /// <returns>A task that resolves to the result DTO representing the deleted entity.</returns>
        Task<TDtoResult> DeleteAsync(TDto dto);
    }
}
