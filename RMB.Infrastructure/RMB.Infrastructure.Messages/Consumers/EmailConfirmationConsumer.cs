using FluentValidation;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Messages.Consumers;
using RMB.Core.Messages.Pipelines;
using RMB.Core.Messages.Resiliences;
using RMB.Infrastructure.Messages.Services;

namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Specialized RabbitMQ consumer for processing email confirmation messages with built-in resilience.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This consumer implements:
    /// - Automatic message deserialization
    /// - FluentValidation integration
    /// - Polly resilience policies (retry/circuit breaker)
    /// - Dead-letter queue routing for failed messages
    /// - Success/failure event publishing
    /// </para>
    /// <para>
    /// Inherits from <see cref="MessageBaseAsyncConsumer{T}"/> to reuse base consumption logic.
    /// </para>
    /// </remarks>
    public class EmailConfirmationConsumer : MessageBaseAsyncConsumer<EmailConfirmationMessage>
    {
        /// <summary>
        /// Initializes a new instance of the email confirmation message consumer.
        /// </summary>
        /// <param name="channel">The RabbitMQ channel for message consumption.</param>
        /// <param name="mailService">The email service for sending confirmations.</param>
        /// <param name="validator">The FluentValidation validator for message validation.</param>
        /// <param name="failureHandler">The dead-letter queue handler for failed messages.</param>
        /// <param name="messageErrorEventPublisher">The error event publisher for monitoring.</param>
        /// <param name="messageSuccessEventPublisher">The success event publisher (optional).</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any required parameter is null.
        /// </exception>
        public EmailConfirmationConsumer(
            IChannel channel,
            MailService mailService,
            IValidator<EmailConfirmationMessage> validator,
            IMessageDeadLetterHandler failureHandler,
            IMessageErrorEventPublisher messageErrorEventPublisher,
            IMessageSuccessEventPublisher messageSuccessEventPublisher)
            : base(
                channel,
                pipeline: MessageProcessingPipeline.Build(
                    validator,
                    async (msg, ct) => await mailService.SendAsync(msg),
                    failureHandler, messageErrorEventPublisher, messageSuccessEventPublisher).InvokeAsync,
                resiliencePolicy: MessagePollyPolicies.CreateResiliencePolicy())
        {
        }
    }
}
