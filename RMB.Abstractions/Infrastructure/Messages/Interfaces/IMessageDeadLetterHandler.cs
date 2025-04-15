
namespace RMB.Abstractions.Infrastructure.Messages.Interfaces
{
    /// <summary>
    /// Defines a contract for handling messages that could not be processed successfully,
    /// typically by forwarding them to a dead letter mechanism for later inspection or retry.
    /// </summary>
    public interface IMessageDeadLetterHandler
    {
        /// <summary>
        /// Handles a failed message by recording or redirecting it based on the failure context.
        /// </summary>
        /// <param name="messageBody">The raw content of the failed message.</param>
        /// <param name="errorDetails">The detailed error message or exception description.</param>
        /// <param name="messageType">The type or classification of the message.</param>
        /// <param name="category">A category indicating the nature of the failure (e.g., deserialization, validation).</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous handling operation.</returns>
        Task HandleAsync(string messageBody, string errorDetails, string messageType, string category, CancellationToken cancellationToken);
    }
}