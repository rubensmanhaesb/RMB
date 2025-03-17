

namespace RMB.Abstractions.Models
{
    public abstract class EntityModel<TKey> : BaseModel, IBaseModel<TKey>
    {
        public virtual TKey Id { get; set; }
        public TKey GetKey()
        {
            return this.Id;
        }
    }
}
