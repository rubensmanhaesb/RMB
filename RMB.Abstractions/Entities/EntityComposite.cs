using RMB.Abstractions.Shared.Models;

namespace RMB.Abstractions.Entities
{
    /// <summary>
    /// Represents an entity that uses a composite key.
    /// This abstract class extends <see cref="Model{TKey}"/>, allowing entities 
    /// with composite keys to inherit common behaviors.
    /// </summary>
    /// <typeparam name="TKey">The type of the composite key.</typeparam>
    public abstract class EntityComposite<TKey> : Model<TKey>
    {
    }
}
