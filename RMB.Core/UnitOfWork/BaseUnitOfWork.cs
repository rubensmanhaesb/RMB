using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMB.Abstractions.Repositories;

namespace RMB.Core.UnitOfWork
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly DbContext _dbContext;
        private  IDbContextTransaction _transaction;

        public BaseUnitOfWork(DbContext context)
        {
            _dbContext = context;
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
