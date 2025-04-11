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
        /// Gets or sets the confirmation link the user should click to verify their email.
        /// </summary>
        public string ConfirmationLink { get; set; }
    }

}
