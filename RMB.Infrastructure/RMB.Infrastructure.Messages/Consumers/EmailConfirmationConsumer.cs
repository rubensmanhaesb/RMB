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
    /// Asynchronous consumer responsible for processing EmailConfirmationMessage instances from RabbitMQ.
    /// Uses the base pipeline structure and applies retry and circuit breaker policies.
    /// </summary>
    public class EmailConfirmationConsumer : MessageBaseAsyncConsumer<EmailConfirmationMessage>
    {
        /// <summary>
        /// Initializes the EmailConfirmationConsumer with the required services and constructs the message processing pipeline.
        /// </summary>
        /// <param name="channel">The RabbitMQ channel used for message consumption.</param>
        /// <param name="mailService">Service responsible for sending confirmation emails.</param>
        /// <param name="validator">Validator for EmailConfirmationMessage objects.</param>
        /// <param name="failureHandler">Handler for messages that fail during processing.</param>
        /// <param name="eventPublisher">Event publisher for background processing failures.</param>
        public EmailConfirmationConsumer(
            IChannel channel,
            MailService mailService,
            IValidator<EmailConfirmationMessage> validator,
            IMessageDeadLetterHandler failureHandler,
            IMessageBackgroundEventPublisher eventPublisher)
            : base(
                channel,
                pipeline: MessageProcessingPipeline.Build(
                    validator,
                    async (msg, ct) => await mailService.SendAsync(msg),
                    failureHandler, eventPublisher).InvokeAsync,
                resiliencePolicy: MessagePollyPolicies.CreateResiliencePolicy())
        {
        }
    }
}
