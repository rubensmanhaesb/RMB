using System;

namespace RMB.Abstractions.Infrastructure.Messages.Entities
{
    /// <summary>
    /// Represents a message that failed during processing and could not be handled successfully.
    /// This entity is typically used for logging, monitoring, or reprocessing purposes.
    /// </summary>
    public class MessageFailure
    {
        /// <summary>
        /// Unique identifier for the failure record.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Name of the source system or application where the message originated.
        /// </summary>
        public string SourceSystem { get; set; }


        /// <summary>
        /// Timestamp (in UTC) indicating when the failure was recorded.
        /// </summary>
        public DateTime FailureTimestamp { get; set; }

        /// <summary>
        /// Optional classification of the failure (e.g., DeserializationError, ValidationError, ProcessingError).
        /// Helps in categorizing and troubleshooting issues.
        /// </summary>
        public string FailureCategory { get; set; }

        /// <summary>
        /// Raw string or serialized representation of the original failed message.
        /// Useful for diagnostics or reprocessing.
        /// </summary>
        public string OriginalFailureMessage { get; set; }

    }
}
