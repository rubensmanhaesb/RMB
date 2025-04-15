using Microsoft.Extensions.Configuration;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using Serilog;

namespace RMB.Core.Messages.FailedMessages.Services
{
    /// <summary>
    /// Implementation of dead letter handler to persist failed message data.
    /// </summary>
    public class MessageDeadLetterHandler : IMessageDeadLetterHandler
    {
        private readonly IMessageFailureService _messageFailureService;
        private readonly string _sourceSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDeadLetterHandler"/> class.
        /// </summary>
        /// <param name="messageFailureService">Service responsible for registering message failures.</param>
        /// <param name="configuration">Application configuration for reading the source system value.</param>
        public MessageDeadLetterHandler(IMessageFailureService messageFailureService, IConfiguration configuration)
        {
            _messageFailureService = messageFailureService;
            _sourceSystem = configuration["MensagemFalha:SistemaOrigem"] ?? "UnknownSystem";
        }

        /// <summary>
        /// Handles the failed message by logging and delegating it to the failure registration service.
        /// </summary>
        /// <param name="messageBody">Raw content of the failed message.</param>
        /// <param name="errorDetails">Details of the failure or exception.</param>
        /// <param name="messageType">The type of the message that failed.</param>
        /// <param name="category">The failure category (e.g., validation, deserialization).</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        public async Task HandleAsync(string messageBody, string errorDetails, string messageType, string category, CancellationToken cancellationToken)
        {
            var failure = new MessageFailure
            {
                Id = Guid.NewGuid(),
                SourceSystem = _sourceSystem,
                FailureTimestamp = DateTime.UtcNow,
                FailureCategory = category
            };

            Log.Warning("Registrando mensagem falha: {MessageType} | {Category}", messageType, category);
            await _messageFailureService.RegisterFailureAsync(failure, cancellationToken);
        }
    }
}