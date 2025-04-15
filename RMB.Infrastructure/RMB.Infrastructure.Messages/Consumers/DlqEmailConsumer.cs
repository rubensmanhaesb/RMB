using Polly;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Core.Messages.Consumers;


namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Dedicated RabbitMQ consumer for handling messages from the Dead Letter Queue (DLQ)
    /// specifically related to the EmailConfirmationMessage type.
    /// </summary>
    public class DlqEmailConsumer : MessageBaseAsyncConsumer<EmailConfirmationMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DlqEmailConsumer"/> class.
        /// </summary>
        /// <param name="channel">The RabbitMQ channel used for communication.</param>
        /// <param name="pipeline">The pipeline responsible for message deserialization, validation, and processing.</param>
        /// <param name="resiliencePolicy">The Polly policy applied for retries and circuit breaker.</param>
        public DlqEmailConsumer(
            IChannel channel,
            Func<ReadOnlyMemory<byte>, CancellationToken, Task<bool>> pipeline,
            AsyncPolicy<bool> resiliencePolicy)
            : base(channel, pipeline, resiliencePolicy)
        {
        }
    }
}
