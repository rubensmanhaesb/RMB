

namespace RMB.Abstractions.Models
{
    public abstract class EntityModel<Tkey> : BaseModel, IBaseModel<Tkey>
    {
        public virtual Tkey Id { get; set; }
        public Tkey GetKey()
        {
            return this.Id;
        }
    }
}
