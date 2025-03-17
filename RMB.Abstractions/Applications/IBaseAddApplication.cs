
namespace RMB.Abstractions.Applications
{
    public interface IBaseAddApplication<TDto, TDtoResult> 
    {
        Task<TDtoResult> AddAsync(TDto dto);
    }
}
