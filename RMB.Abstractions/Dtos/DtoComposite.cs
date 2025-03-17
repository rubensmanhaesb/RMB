using RMB.Abstractions.Models;

namespace RMB.Abstractions.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) with a composite key.
    /// This abstract class extends <see cref="EntityModel{TKey}"/> to provide a 
    /// base structure for DTOs that use composite keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the composite key.</typeparam>
    public abstract class DtoComposite<TKey> : EntityModel<TKey>
    {
    }
}
