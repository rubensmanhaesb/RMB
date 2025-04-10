using FluentValidation;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages;
using RMB.Core.Messages.Consumers;
using RMB.Core.Messages.Pipelines;
using RMB.Core.Messages.Resiliences;
using RMB.Infrastructure.Messages.Helpers;

namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Asynchronous consumer for processing email confirmation messages from RabbitMQ.
    /// Leverages the base pipeline architecture and resilience policies.
    /// </summary>
    public class EmailConfirmationConsumer : BaseAsyncConsumer<EmailConfirmationMessage>
    {
        /// <summary>
        /// Constructs the email confirmation consumer with the required services and dependencies.
        /// </summary>
        /// <param name="channel">RabbitMQ channel instance.</param>
        /// <param name="mailHelper">Helper responsible for sending email messages.</param>
        /// <param name="validator">Validator for email confirmation messages.</param>
        /// <param name="logger">Logger instance for logging errors and diagnostics.</param>
        public EmailConfirmationConsumer(
            IChannel channel,
            MailHelper mailHelper,
            IValidator<EmailConfirmationMessage> validator,
            ILogger<MailMessageConsumer> logger)
            : base(
                channel,
                pipeline: MessageProcessingPipeline.Build(
                    validator,
                    async (msg, ct) => await mailHelper.SendAsync(msg),
                    logger).InvokeAsync,
                resiliencePolicy: PollyPolicies.CreateResiliencePolicy(logger))
        {
        }
    }
}
