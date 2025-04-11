using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;

namespace RMB.Core.Messages.FailedMessages.Services
{
    /// <summary>
    /// Default implementation of the IMessageFailureService that stores failed messages using a persistence strategy.
    /// </summary>
    public class MessageFailureService : IMessageFailureService
    {
        private readonly ILogger<MessageFailureService> _logger;
        private readonly string _sourceSystem;

        public MessageFailureService(ILogger<MessageFailureService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _sourceSystem = configuration["MensagemFalha:SistemaOrigem"] ?? "UnknownSystem";
        }

        /// <summary>
        /// Registers the failure by persisting or logging it.
        /// </summary>
        public async Task RegisterFailureAsync(MessageFailure failure, CancellationToken cancellationToken)
        {
            failure.Id = Guid.NewGuid();
            failure.FailureTimestamp = DateTime.UtcNow;
            failure.SourceSystem = _sourceSystem;

            _logger.LogError("[Message Failure] Tipo: {MessageType}, Erro: {Error}, Payload: {Content}", failure.MessageType, failure.ErrorDetails, failure.Content);

            await Task.CompletedTask; // Simulate async persistence
        }
    }
}
