using RMB.Abstractions.Infrastructure.Messages.Interfaces;


namespace RMB.Core.Messages.Events.BackgroundEventPublisher
{
    /// <summary>
    /// Default implementation of the <see cref="IMessageErrorEventPublisher"/> interface.
    /// This class allows publishing background error events to any subscribed handlers.
    /// </summary>
    /// <remarks>
    /// Provides a mechanism for centralized error handling in background message processing,
    /// enabling decoupled error notification and handling across the application.
    /// </remarks>
    public class MessageErrorEventPublisher : IMessageErrorEventPublisher
    {
        /// <summary>
        /// Event triggered when an error occurs in the background message processing.
        /// Subscribers can handle the exception accordingly.
        /// </summary>
        /// <value>
        /// A nullable event that subscribers can register with to receive error notifications.
        /// The event provides both the sender and the <see cref="Exception"/> that occurred.
        /// </value>
        public event EventHandler<Exception>? OnError;

        /// <summary>
        /// Publishes an error event to any registered event handlers.
        /// </summary>
        /// <param name="message">The contextual message or additional information about the error.</param>
        /// <param name="exception">The exception that occurred during processing.</param>
        /// <remarks>
        /// <para>
        /// Safely invokes the <see cref="OnError"/> event if there are subscribers.
        /// The current instance (<c>this</c>) is passed as the sender along with the exception.
        /// </para>
        /// <para>
        /// Note: While the <paramref name="message"/> parameter is accepted, it is currently not used
        /// in the event payload. Consider extending the event args if message details are needed by subscribers.
        /// </para>
        /// </remarks>
        /// <example>
        /// Typical usage:
        /// <code>
        /// var publisher = new MessageErrorEventPublisher();
        /// publisher.OnError += (sender, ex) => logger.Error(ex, "Background processing failed");
        /// try {
        ///     // processing logic
        /// } catch (Exception ex) {
        ///     publisher.PublishError("Failed to process message", ex);
        /// }
        /// </code>
        /// </example>
        public void PublishError(string message, Exception exception)
        {

            OnError?.Invoke(this, exception);

        }
    }

}
