using RMB.Abstractions.Infrastructure.Messages.Interfaces;


namespace RMB.Core.Messages.Events.BackgroundEventPublisher
{
    /// <summary>
    /// Default implementation of the IMessageBackgroundEventPublisher interface.
    /// This class allows publishing background error events to any subscribed handlers.
    /// </summary>
    public class MessageBackgroundEventPublisher : IMessageBackgroundEventPublisher
    {
        /// <summary>
        /// Event triggered when an error occurs in the background message processing.
        /// Subscribers can handle the exception accordingly.
        /// </summary>
        public event EventHandler<Exception>? OnError;

        /// <summary>
        /// Publishes an error event to any registered event handlers.
        /// </summary>
        /// <param name="message">The contextual message or additional information.</param>
        /// <param name="exception">The exception that occurred.</param>
        public void PublishError(string message, Exception exception)
        {

            OnError?.Invoke(this, exception);

        }
    }

}
