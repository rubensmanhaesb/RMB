using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RMB.Infrastructure.Messages.Helpers
{
    /// <summary>
    /// Utility class responsible for creating and configuring RabbitMQ queues,
    /// including Dead Letter Exchange (DLX) and Dead Letter Queue (DLQ) support.
    /// </summary>
    public class QueueInitializer
    {
        private readonly IChannel _channel;

        /// <summary>
        /// Initializes the QueueInitializer with a RabbitMQ channel instance.
        /// </summary>
        /// <param name="channel">The RabbitMQ channel used to declare queues and exchanges.</param>
        public QueueInitializer(IChannel channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// Ensures that the specified queue is declared with a DLX and DLQ.
        /// Creates the DLX exchange, the DLQ, and binds them accordingly.
        /// </summary>
        /// <param name="queueName">The name of the primary queue.</param>
        /// <param name="cancellationToken">Token to cancel the async operations if needed.</param>
        public async Task EnsureQueueWithDeadLetterAsync(string queueName, CancellationToken cancellationToken)
        {
            var dlxExchange = $"{queueName}.dlx";
            var dlqName = $"{queueName}.dlq";

            // Declare Dead Letter Exchange (DLX)
            await _channel.ExchangeDeclareAsync(
                exchange: dlxExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            // Declare Dead Letter Queue (DLQ)
            await _channel.QueueDeclareAsync(
                queue: dlqName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            // Bind DLQ to DLX using the DLQ name as routing key
            await _channel.QueueBindAsync(
                queue: dlqName,
                exchange: dlxExchange,
                routingKey: dlqName,
                cancellationToken: cancellationToken);

            // Declare main queue with DLX configuration
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", dlxExchange },
                { "x-dead-letter-routing-key", dlqName }
            };

            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments,
                cancellationToken: cancellationToken);
        }
    }
}


