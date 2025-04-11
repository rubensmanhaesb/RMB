using RMB.Abstractions.Infrastructure.Messages.Entities;


namespace RMB.Abstractions.Infrastructure.Messages.Interfaces
{
    /// <summary>
    /// Defines a service contract for recording failed message processing attempts.
    /// </summary>
    public interface IMessageFailureService
    {
        /// <summary>
        /// Persists a message failure to a storage mechanism (e.g., database, file, logging system).
        /// </summary>
        /// <param name="failure">The failed message entity to be saved.</param>
        /// <param name="cancellationToken">A cancellation token for the async operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RegisterFailureAsync(MessageFailure failure, CancellationToken cancellationToken);
    }
}
