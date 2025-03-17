namespace RMB.Abstractions.Shared.Models
{
    /// <summary>
    /// Represents a base entity model with a composite key.
    /// This abstract class extends <see cref="BaseModel"/> and implements <see cref="IBaseModel{TKey}"/>,
    /// providing a common structure for entity models that use a composite key.
    /// </summary>
    /// <typeparam name="TKey">The type of the composite key.</typeparam>
    public abstract class CompositeModel<TKey> : BaseModel, IBaseModel<TKey>
    {
        /// <summary>
        /// Gets or sets the composite key for the entity.
        /// </summary>
        public virtual TKey Key { get; set; }

        /// <summary>
        /// Retrieves the composite key of the entity.
        /// </summary>
        /// <returns>The composite key of the entity.</returns>
        public TKey GetKey()
        {
            return Key;
        }
    }
}
