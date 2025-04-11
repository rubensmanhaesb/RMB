using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using System.Text.Json;

namespace RMB.Core.Messages.FailedMessages.Services
{
    /// <summary>
    /// Implementation of dead letter handler to persist failed message data.
    /// </summary>
    public class DeadLetterHandler : IDeadLetterHandler
    {
        private readonly IMessageFailureService _messageFailureService;
        private readonly ILogger<DeadLetterHandler> _logger;
        private readonly string _sourceSystem;

        public DeadLetterHandler(IMessageFailureService messageFailureService, ILogger<DeadLetterHandler> logger, IConfiguration configuration)
        {
            _messageFailureService = messageFailureService;
            _logger = logger;
            _sourceSystem = configuration["MensagemFalha:SistemaOrigem"] ?? "UnknownSystem";
        }

        public async Task HandleAsync(string messageBody, string errorDetails, string messageType, string category, CancellationToken cancellationToken)
        {
            var failure = new MessageFailure
            {
                Id = Guid.NewGuid(),
                SourceSystem = _sourceSystem,
                MessageType = messageType,
                Content = messageBody,
                ErrorDetails = errorDetails,
                FailureTimestamp = DateTime.UtcNow,
                FailureCategory = category
            };

            _logger.LogWarning("Registrando mensagem falha: {MessageType} | {Category}", messageType, category);
            await _messageFailureService.RegisterFailureAsync(failure, cancellationToken);
        }
    }
}