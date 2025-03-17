using RMB.Abstractions.Models;

namespace RMB.Abstractions.Entities
{
    /// <summary>
    /// Represents a generic entity with a unique identifier.
    /// This abstract class extends <see cref="EntityModel{TKey}"/>, 
    /// serving as a base for domain entities with a single primary key.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public abstract class Entity<TKey> : EntityModel<TKey>
    {
    }
}
