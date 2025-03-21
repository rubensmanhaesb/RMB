namespace RMB.Abstractions.Entities
{
    /// <summary>
    /// Abstract base class representing a generic entity with a typed primary key.
    /// Inherits from BaseEntity and implements IBaseEntity interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public abstract class Entity<TKey> : BaseEntity, IBaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Returns the unique identifier of the entity.
        /// </summary>
        /// <returns>The primary key value.</returns>
        public TKey GetKey()
        {
            return Id;
        }
    }
}
