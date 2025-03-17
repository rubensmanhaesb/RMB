
namespace RMB.Abstractions.Models
{
    public abstract class EntityCompositeModel<Tkey> : BaseModel, IBaseModel<Tkey>
    {
        public virtual Tkey Key { get; set; }

        public Tkey GetKey()
        {
            return this.Key;
        }
    }
}
