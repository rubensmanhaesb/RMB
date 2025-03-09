using RMB.Abstractions.Domains;
using RMB.Abstractions.Models;
using RMB.Core.Repositories;

namespace RMB.Core.Domains
{
    public abstract class BaseDomain<TEntity> :
        IBaseAddDomain<TEntity>,
        IBaseUpdateDomain<TEntity>,
        IBaseDeleteDomain<TEntity>,
        IBaseQueryDomain<TEntity> where TEntity : BaseModel
    {
        private readonly BaseRepository<TEntity> _baseRepository;

        protected BaseDomain(BaseRepository<TEntity> repository)
        {
            _baseRepository = repository;
        }

        public async virtual Task<TEntity> AddAsync(TEntity entity)
        {
            return await _baseRepository.AddAsync(entity);
        }

        public async virtual Task<TEntity> DeleteAsync(TEntity entity)
        {
            return await _baseRepository.DeleteAsync(entity);
        }

        public void Dispose()
        {
            _baseRepository.Dispose();
        }

        public async virtual Task<List<TEntity>>? GetAll()
        {
            return await _baseRepository.GetAll();
        }

        public async virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await _baseRepository.UpdateAsync(entity);
        }
    }
}
