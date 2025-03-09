
namespace RMB.Abstractions.Models
{
    internal interface IBaseModel<TKey>
    {
        TKey GetKey();
    }
}
