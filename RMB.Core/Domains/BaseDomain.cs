using Microsoft.EntityFrameworkCore;
using RMB.Abstractions.Domains;
using RMB.Abstractions.Shared.Models;
using RMB.Core.Repositories;
using System.Linq.Expressions;

namespace RMB.Core.Domains
{
    /// <summary>
    /// Base class for domain logic, providing CRUD operations using a repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from BaseModel.</typeparam>
    public abstract class BaseDomain<TEntity> :
        IBaseAddDomain<TEntity>,
        IBaseUpdateDomain<TEntity>,
        IBaseDeleteDomain<TEntity>,
        IBaseQueryDomain<TEntity> 
        where TEntity : BaseModel
    {
        private readonly BaseRepository<TEntity> _baseRepository;

        protected BaseDomain(BaseRepository<TEntity> repository)
            =>_baseRepository = repository;

        public async virtual Task<TEntity> AddAsync(TEntity entity)
            => await _baseRepository.AddAsync(entity);
        

        public async virtual Task<TEntity> DeleteAsync(TEntity entity)
            => await _baseRepository.DeleteAsync(entity);


        public void Dispose()
            => _baseRepository.Dispose();
        

        public async virtual Task<List<TEntity>>? GetAllAsync()
            => await _baseRepository.GetAllAsync();

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
            => await _baseRepository.GetAllAsync(predicate);

        public async Task<TEntity>? GetByIdAsync(Guid id)
            => await _baseRepository.GetByIdAsync(id);

        public Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate)
            => _baseRepository.GetOneByAsync(predicate);

        public async virtual Task<TEntity> UpdateAsync(TEntity entity)
            => await _baseRepository.UpdateAsync(entity);
        

    }
}
