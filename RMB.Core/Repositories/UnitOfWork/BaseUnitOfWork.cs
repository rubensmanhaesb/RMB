using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMB.Abstractions.Repositories;

namespace RMB.Core.Repositories.UnitOfWork
{
    /// <summary>
    /// Base implementation of the Unit of Work pattern, managing database transactions and ensuring atomic operations.
    /// This class provides methods to begin, commit, and rollback transactions, as well as save changes asynchronously.
    /// </summary>
    public abstract class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly DbContext _dbContext;
        private  IDbContextTransaction _transaction;

        public BaseUnitOfWork(IDbContextFactory<DbContext> factory)
        {
            _dbContext = factory.CreateDbContext(); 
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;

        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();

        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
