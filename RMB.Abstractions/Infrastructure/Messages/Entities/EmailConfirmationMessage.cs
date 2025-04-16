namespace RMB.Abstractions.Infrastructure.Messages.Entities
{
    /// <summary>
    /// Represents the message payload for confirming a user's email address.
    /// </summary>
    public class EmailConfirmationMessage
    {
        /// <summary>
        /// Gets or sets the destination email address for the confirmation.
        /// </summary>
        public string ToEmail { get; set; }


        /// <summary>
        /// Gets or sets the name of the recipient (to personalize the email).
        /// </summary>
        public string? ToName { get; set; }

        /// <summary>
        /// Gets or sets the internal identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the datetime when the confirmation email was requested.
        /// </summary>
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the expiration date/time of the confirmation link.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the HTML body of the email.
        /// </summary>
        public string Body { get; set; }


    }


}
