using RMB.Abstractions.Shared.Models;

namespace RMB.Abstractions.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) with a unique identifier.
    /// This abstract class extends <see cref="Model{TKey}"/> to provide a 
    /// base structure for DTOs in the application.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public abstract class Dto<TKey> : Model<TKey>
    {
    }
}
