using RMB.Abstractions.Core;
using RMB.Abstractions.Models;


namespace RMB.Abstractions.Applications
{

    public interface IBaseDeleteApplication<TDto, TDtoResult> 
    {
        Task<TDtoResult> DeleteAsync(TDto dto);
    }
}
