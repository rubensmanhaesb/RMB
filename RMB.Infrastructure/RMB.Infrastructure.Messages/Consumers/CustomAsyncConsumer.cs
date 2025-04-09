using FluentValidation;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages;
using RMB.Core.Messages.Pipelines;
using RMB.Core.Messages.Resiliences;
using RMB.Infrastructure.Messages.Helpers;


namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Asynchronous RabbitMQ consumer that handles message processing with middleware, validation, retry, and circuit breaker.
    /// </summary>
    internal class CustomAsyncConsumer : AsyncDefaultBasicConsumer
    {
        private readonly Func<ReadOnlyMemory<byte>, CancellationToken, Task<bool>> _pipeline;
        private readonly AsyncPolicy<bool> _resiliencePolicy;

        /// <summary>
        /// Initializes the consumer with the message processing pipeline and resilience policy.
        /// </summary>
        /// <param name="channel">RabbitMQ channel for communication.</param>
        /// <param name="mailHelper">Helper to send email messages.</param>
        /// <param name="validator">Validator for the deserialized message.</param>
        /// <param name="logger">Logger for error and info logging.</param>
        public CustomAsyncConsumer(
            IChannel channel,
            MailHelper mailHelper,
            IValidator<EmailConfirmationMessage> validator,
            ILogger<MailMessageConsumer> logger
        ) : base(channel)
        {
            // Build the middleware pipeline for message handling
            _pipeline = MessageProcessingPipeline.Build<EmailConfirmationMessage>(
                validator,
                async (msg, ct) => await mailHelper.SendAsync(msg),
                logger
            ).InvokeAsync;

            // Configure Polly retry and circuit breaker policies
            _resiliencePolicy = PollyPolicies.CreateResiliencePolicy(logger);
        }

        /// <summary>
        /// Handles the delivery of a message from the RabbitMQ queue.
        /// It processes the message using the middleware pipeline and applies resilience policies.
        /// </summary>
        /// <param name="consumerTag">The consumer tag associated with the consumer.</param>
        /// <param name="deliveryTag">The delivery tag for the message.</param>
        /// <param name="redelivered">Whether the message was redelivered.</param>
        /// <param name="exchange">The exchange the message was published to.</param>
        /// <param name="routingKey">The routing key used when the message was published.</param>
        /// <param name="properties">Basic properties of the message.</param>
        /// <param name="body">The message body as a byte array.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
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
            // Execute the message pipeline with retry and circuit breaker
            var success = await _resiliencePolicy.ExecuteAsync(() =>
                _pipeline(body, cancellationToken));

            if (success)
            {
                // Acknowledge the message was processed successfully
                await Channel.BasicAckAsync(deliveryTag, multiple: false, cancellationToken);
            }
            else
            {
                // Reject the message and do not requeue
                await Channel.BasicNackAsync(deliveryTag, multiple: false, requeue: false, cancellationToken);
            }
        }
    }
}
