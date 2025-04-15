using RMB.Abstractions.Infrastructure.Messages.Entities;

namespace RMB.Abstractions.Infrastructure.Messages.Exceptions         
{
    /// <summary>
    /// Exception thrown when deserialization of the incoming message fails.
    /// Used to capture and handle malformed or incompatible JSON payloads.
    /// </summary>
    public class MessageDeserializationException : MessagesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDeserializationException"/> class.
        /// </summary>
        /// <param name="message">The error message describing the deserialization issue.</param>
        /// <param name="originalJson">The raw JSON string that failed to deserialize.</param>
        /// <param name="innerException">Optional inner exception for stack trace and debugging purposes.</param>
        public MessageDeserializationException(
            string message,
            string originalJson,
            Exception? innerException = null)
            : base(message, originalJson, innerException)
        { }

    }
}
