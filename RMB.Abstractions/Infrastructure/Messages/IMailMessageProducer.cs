using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMB.Abstractions.Infrastructure.Messages
{
    /// <summary>
    /// Defines a contract for publishing email confirmation messages to a message broker.
    /// </summary>
    public interface IMailMessageProducer
    {
        /// <summary>
        /// Publishes an email confirmation message asynchronously to a queue.
        /// </summary>
        /// <param name="message">The email confirmation message to be published.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        Task PublishEmailConfirmationAsync(EmailConfirmationMessage message);
    }

}
