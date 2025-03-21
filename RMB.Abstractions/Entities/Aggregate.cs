namespace RMB.Abstractions.Entities
{
    /// <summary>
    /// Abstract base class representing an aggregate root in the domain model.
    /// Inherits from BaseEntity and implements IBaseEntity with a typed key.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public abstract class Aggregate<TKey> : BaseEntity, IBaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the unique key of the aggregate.
        /// </summary>
        public virtual TKey Key { get; set; }

        /// <summary>
        /// Returns the unique key of the aggregate.
        /// </summary>
        /// <returns>The primary key value.</returns>
        public TKey GetKey()
        {
            return Key;
        }
    }
}
