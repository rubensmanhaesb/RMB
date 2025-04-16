using RMB.Abstractions.Infrastructure.Messages.Entities;

namespace RMB.Abstractions.Infrastructure.Messages.Interfaces
{
    /// <summary>
    /// Defines a publisher for successful message processing events.
    /// </summary>
    /// <remarks>
    /// This interface provides a mechanism to notify subscribers when email confirmation messages
    /// are successfully processed, enabling event-driven workflows.
    /// </remarks>
    public interface IMessageSuccessEventPublisher
    {
        /// <summary>
        /// Event that is raised when an email confirmation is successfully processed.
        /// </summary>
        /// <value>
        /// Subscribers can attach handlers to be notified of successful email confirmations.
        /// The event provides the complete <see cref="EmailConfirmationMessage"/> that was processed.
        /// </value>
        event EventHandler<EmailConfirmationMessage> OnSuccess;

        /// <summary>
        /// Publishes a notification about a successfully processed email confirmation.
        /// </summary>
        /// <param name="emailConfirmationMessage">The email confirmation message that was processed successfully.</param>
        /// <remarks>
        /// This method triggers the <see cref="OnSuccess"/> event, notifying all subscribers
        /// about the successful processing of the email confirmation.
        /// </remarks>
        void PublishSuccess(EmailConfirmationMessage emailConfirmationMessage);
    }

}
