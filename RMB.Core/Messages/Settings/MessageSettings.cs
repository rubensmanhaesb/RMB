
namespace RMB.Core.Messages.Settings
{
    /// <summary>
    /// Configuration settings for message-related operations.
    /// </summary>
    public class MessageSettings
    {
        /// <summary>
        /// Base URL used to generate the confirmation link sent in emails.
        /// </summary>
        public string ConfirmationLinkBaseUrl { get; set; } = string.Empty;
    }
}
