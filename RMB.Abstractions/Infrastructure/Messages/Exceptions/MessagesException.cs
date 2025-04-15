using RMB.Abstractions.Infrastructure.Messages.Entities;

namespace RMB.Abstractions.Infrastructure.Messages.Exceptions
{
    /// <summary>
    /// Base class for all custom exceptions related to message processing.
    /// Provides common context including original JSON, parsed message, and failure metadata.
    /// </summary>
    public abstract class MessagesException : Exception
    {
        /// <summary>
        /// The raw JSON content that caused the exception.
        /// </summary>
        public string OriginalJson { get; init; } = string.Empty;

        /// <summary>
        /// The deserialized message instance (if available) that triggered the exception.
        /// </summary>
        public EmailConfirmationMessage? EmailConfirmationMessage { get; init; }

        /// <summary>
        /// Metadata used for logging or persistence of the failure (e.g., for DLQ handling).
        /// </summary>
        public MessageFailure? MessageFailure  { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="originalJson">The raw JSON that caused the error.</param>
        /// <param name="emailConfirmationMessage">The deserialized message object, if available.</param>
        /// <param name="messageFailure">Metadata for tracking the failure.</param>
        /// <param name="innerException">Optional inner exception.</param>
        protected MessagesException(
            string message,
            string originalJson,
            EmailConfirmationMessage? emailConfirmationMessage = null,
            MessageFailure? messageFailure = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            EmailConfirmationMessage = emailConfirmationMessage;
            MessageFailure = messageFailure;
            OriginalJson = originalJson;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesException"/> class
        /// without explicitly passing parsed message or failure details.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="originalJson">The raw JSON that caused the error.</param>
        /// <param name="innerException">Optional inner exception.</param>
        protected MessagesException(
            string message,
            string originalJson,
            Exception? innerException = null)
            : base(message, innerException)
        {
            OriginalJson = originalJson;
        }
    }
}
