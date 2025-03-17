using AutoMapper;
using MediatR;
using RMB.Abstractions.UseCases;
using RMB.Core.Domains;
using RMB.Abstractions.Shared.Models;

namespace RMB.Core.Application
{
    /// <summary>
    /// Base class for Mediator-based application services with CRUD operations.
    /// </summary>
    public abstract class BaseMediatorApplication<TDtoCreate, TDtoUpdate, TDtoDelete, TDtoResult, TEntity> :
        IBaseAddUseCase<TDtoCreate, TDtoResult>,
        IBaseUpdateUseCase<TDtoUpdate, TDtoResult>,
        IBaseDeleteUseCase<TDtoDelete, TDtoResult>
        where TEntity : BaseModel
    {
        private readonly BaseDomain<TEntity> _baseDomain;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        protected BaseMediatorApplication(BaseDomain<TEntity> baseDomain, IMediator mediator, IMapper mapper)
        {
            _baseDomain = baseDomain;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<TDtoResult> AddAsync(TDtoCreate dto)
            => (TDtoResult) await _mediator.Send(dto);

        public async Task<TDtoResult> DeleteAsync(TDtoDelete dto)
            => (TDtoResult) await _mediator.Send(dto);
        public async Task<TDtoResult> UpdateAsync(TDtoUpdate dto)
            => (TDtoResult) await _mediator.Send(dto);

        public virtual async Task<List<TDtoResult>?> GetAllAsync()
        {
            var lista = await _baseDomain.GetAllAsync();
            var result = _mapper.Map<List<TDtoResult>>(lista);

            return result;
        }

        public virtual async Task<TDtoResult?> GetByIdAsync(Guid id)
        {
            var pessoaJuridicaDto = await _baseDomain.GetByIdAsync(id);
            var result = _mapper.Map<TDtoResult>(pessoaJuridicaDto);

            return result;
        }

    }
}
