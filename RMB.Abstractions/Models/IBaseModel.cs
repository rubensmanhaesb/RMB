namespace RMB.Abstractions.Models
{
    /// <summary>
    /// Defines a base contract for models with a primary key.
    /// This interface ensures that all implementing entities provide a method 
    /// to retrieve their unique key.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    internal interface IBaseModel<TKey>
    {
        /// <summary>
        /// Retrieves the unique key of the entity.
        /// </summary>
        /// <returns>The primary key of the entity.</returns>
        TKey GetKey();
    }
}
