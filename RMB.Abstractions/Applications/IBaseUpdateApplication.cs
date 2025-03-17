using RMB.Abstractions.Core;
using RMB.Abstractions.Models;


namespace RMB.Abstractions.Applications
{
    public interface IBaseUpdateApplication<TDto, TDtoResult> 
    {
        Task<TDtoResult> UpdateAsync(TDto dto);
    }
}
