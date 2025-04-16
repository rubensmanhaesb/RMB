using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;


namespace RMB.Core.Messages.Events
{

    /// <summary>
    /// Implementation of <see cref="IMessageSuccessEventPublisher"/> for handling successful email confirmation events.
    /// </summary>
    /// <remarks>
    /// This class provides the concrete implementation for publishing notifications when email confirmations
    /// are successfully processed. It follows the event-driven pattern to decouple senders and receivers.
    /// </remarks>
    public class MessageSuccessEventPublisher : IMessageSuccessEventPublisher
    {
        /// <summary>
        /// Event that is raised when an email confirmation is successfully processed.
        /// </summary>
        /// <value>
        /// This nullable event allows subscribers to register handlers for successful email confirmation notifications.
        /// The event provides both the sender and the complete <see cref="EmailConfirmationMessage"/> payload.
        /// </value>        
        public event EventHandler<EmailConfirmationMessage>? OnSuccess;

        /// <summary>
        /// Notifies subscribers about a successfully processed email confirmation message.
        /// </summary>
        /// <param name="emailConfirmationMessage">The successfully processed email confirmation message.</param>
        /// <remarks>
        /// This method safely invokes the <see cref="OnSuccess"/> event if there are any subscribers.
        /// The current instance (<c>this</c>) is passed as the sender along with the message payload.
        /// </remarks>
        /// <example>
        /// Typical usage:
        /// <code>
        /// var publisher = new MessageSuccessEventPublisher();
        /// publisher.OnSuccess += (sender, message) => Console.WriteLine($"Processed: {message.ToEmail}");
        /// publisher.PublishSuccess(confirmationMessage);
        /// </code>
        /// </example>
        public void PublishSuccess(EmailConfirmationMessage emailConfirmationMessage)
        {
            OnSuccess?.Invoke(this, emailConfirmationMessage);
        }
    }

}
