using Polly;
using RabbitMQ.Client;

namespace RMB.Core.Messages.Consumers
{
    /// <summary>
    /// Base class for asynchronous RabbitMQ consumers with retry and circuit breaker support.
    /// Handles ACK/NACK and delegates message processing through a provided pipeline.
    /// </summary>
    /// <typeparam name="TMessage">The type of message being processed.</typeparam>
    public abstract class MessageBaseAsyncConsumer<TMessage> : AsyncDefaultBasicConsumer
        where TMessage : class
    {
        private readonly Func<ReadOnlyMemory<byte>, CancellationToken, Task<bool>> _pipeline;
        private readonly AsyncPolicy<bool> _resiliencePolicy;

        /// <summary>
        /// Initializes the base consumer with a message processing pipeline and a resilience policy.
        /// </summary>
        /// <param name="channel">RabbitMQ channel to communicate with.</param>
        /// <param name="pipeline">The middleware pipeline to process the raw message.</param>
        /// <param name="resiliencePolicy">The Polly policy to handle retries and circuit breaking.</param>
        protected MessageBaseAsyncConsumer(
            IChannel channel,
            Func<ReadOnlyMemory<byte>, CancellationToken, Task<bool>> pipeline,
            AsyncPolicy<bool> resiliencePolicy)
            : base(channel)
        {
            _pipeline = pipeline;
            _resiliencePolicy = resiliencePolicy;
        }

        /// <summary>
        /// Handles incoming messages from RabbitMQ, applying retry policies and ACK/NACK appropriately.
        /// </summary>
        public override async Task HandleBasicDeliverAsync(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IReadOnlyBasicProperties properties,
            ReadOnlyMemory<byte> body,
            CancellationToken cancellationToken = default)
        {
            var success = await _resiliencePolicy.ExecuteAsync(() =>
                _pipeline(body, cancellationToken));

            if (success)
            {
                await Channel.BasicAckAsync(deliveryTag, multiple: false, cancellationToken);
            }
            else
            {
                await Channel.BasicNackAsync(deliveryTag, multiple: false, requeue: false, cancellationToken);
            }
        }
    }
}
