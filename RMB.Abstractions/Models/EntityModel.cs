namespace RMB.Abstractions.Models
{
    /// <summary>
    /// Represents a base entity model with a unique identifier.
    /// This abstract class extends <see cref="BaseModel"/> and implements <see cref="IBaseModel{TKey}"/>,
    /// providing a common structure for entity models with a primary key.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public abstract class EntityModel<TKey> : BaseModel, IBaseModel<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Retrieves the unique key of the entity.
        /// </summary>
        /// <returns>The primary key of the entity.</returns>
        public TKey GetKey()
        {
            return this.Id;
        }
    }
}
