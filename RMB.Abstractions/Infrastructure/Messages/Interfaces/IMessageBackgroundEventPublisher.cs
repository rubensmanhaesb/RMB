

namespace RMB.Abstractions.Infrastructure.Messages.Interfaces
{
    /// <summary>
    /// Defines a contract for publishing events that occur during background message processing.
    /// Used to notify consumers about unhandled exceptions.
    /// </summary>
    public interface IMessageBackgroundEventPublisher
    {
        /// <summary>
        /// Event triggered when an error occurs during background processing.
        /// </summary>
        event EventHandler<Exception> OnError;

        /// <summary>
        /// Publishes an error event to notify subscribers of a background processing failure.
        /// </summary>
        /// <param name="message">The original message that triggered the exception.</param>
        /// <param name="exception">The exception that was thrown during processing.</param>
        void PublishError(string message, Exception exception);
    }
}
