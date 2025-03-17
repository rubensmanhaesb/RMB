
namespace RMB.Abstractions.Models
{
    public abstract class EntityCompositeModel<TKey> : BaseModel, IBaseModel<TKey>
    {
        public virtual TKey Key { get; set; }

        public TKey GetKey()
        {
            return this.Key;
        }
    }
}
