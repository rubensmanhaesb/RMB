using Microsoft.Extensions.Configuration;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using Serilog;

namespace RMB.Core.Messages.FailedMessages.Services
{
    /// <summary>
    /// Default implementation of the IMessageFailureService that stores failed messages using a persistence strategy.
    /// </summary>
    public class MessageFailureService : IMessageFailureService
    {
        private readonly string _sourceSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFailureService"/> class with configuration settings.
        /// </summary>
        /// <param name="configuration">The application configuration containing the source system name.</param>
        public MessageFailureService(IConfiguration configuration)
        {
            _sourceSystem = configuration["MensagemFalha:SistemaOrigem"] ?? "UnknownSystem";
        }

        /// <summary>
        /// Registers the message failure by logging it. This method can be extended to persist the failure to a database or external system.
        /// </summary>
        /// <param name="failure">The failure object containing message failure details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>

        public async Task RegisterFailureAsync(MessageFailure failure, CancellationToken cancellationToken)
        {
            failure.Id = Guid.NewGuid();
            failure.FailureTimestamp = DateTime.UtcNow;
            failure.SourceSystem = _sourceSystem;

            Log.Error("Erro Inesperado {SourceSystem}, ID {Id}, Descrição - ", failure.SourceSystem, failure.Id );

            await Task.CompletedTask; 
        }
    }
}
