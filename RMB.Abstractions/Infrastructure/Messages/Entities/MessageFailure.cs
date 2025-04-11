using System;

namespace RMB.Abstractions.Infrastructure.Messages.Entities
{
    /// <summary>
    /// Represents a failed message that could not be processed successfully.
    /// This entity is used for logging or reprocessing purposes.
    /// </summary>
    public class MessageFailure
    {
        /// <summary>
        /// Unique identifier of the failure entry.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// System or application name that generated the message.
        /// </summary>
        public string SourceSystem { get; set; }

        /// <summary>
        /// Type or name of the message that failed.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Raw content of the message, usually in JSON format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Details about why the message failed (validation errors, exceptions, etc).
        /// </summary>
        public string ErrorDetails { get; set; }

        /// <summary>
        /// Date and time when the message failure was recorded (UTC).
        /// </summary>
        public DateTime FailureTimestamp { get; set; }

        /// <summary>
        /// Optional classification of the failure (e.g., DeserializationError, ValidationError, ProcessingError).
        /// </summary>
        public string FailureCategory { get; set; }
    }
}
