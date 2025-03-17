namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for the Unit of Work pattern.
    /// This interface ensures transaction management and database operations 
    /// are handled atomically.
    /// </summary>
    public interface IBaseUnitOfWork : IDisposable
    {
        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// Ensures that all changes within the transaction are persisted.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// Reverts any changes made within the transaction.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RollbackAsync();

        /// <summary>
        /// Saves all changes made in the current unit of work asynchronously.
        /// If a transaction is active, the changes are part of it.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();
    }
}
