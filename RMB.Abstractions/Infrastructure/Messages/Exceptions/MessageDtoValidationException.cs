using RMB.Abstractions.Infrastructure.Messages.Entities;

namespace RMB.Abstractions.Infrastructure.Messages.Exceptions
{
    /// <summary>
    /// Exception thrown when a DTO fails FluentValidation or custom validation logic.
    /// Captures the deserialized object and failure metadata for troubleshooting and DLQ handling.
    /// </summary>
    public class MessageDtoValidationException : MessagesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDtoValidationException"/> class.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        /// <param name="originalMessage">The raw JSON message that caused the failure.</param>
        /// <param name="emailConfirmationMessage">The deserialized DTO that failed validation.</param>
        /// <param name="failureDetails">Optional failure metadata for logging or persistence.</param>
        /// <param name="innerException">Optional inner exception for stack trace context.</param>
        public MessageDtoValidationException(
            string message,
            string originalMessage,
            EmailConfirmationMessage emailConfirmationMessage,
            MessageFailure? failureDetails = null,
            Exception? innerException = null)
            : base(message, originalMessage, emailConfirmationMessage, failureDetails, innerException)
        {
        }
    }
}
