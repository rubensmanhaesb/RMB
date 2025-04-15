using RabbitMQ.Client;


namespace RMB.Core.Messages.Queues
{
    /// <summary>
    /// Utility class responsible for creating and configuring RabbitMQ queues,
    /// including Dead Letter Exchange (DLX) and Dead Letter Queue (DLQ) support.
    /// </summary>
    public class MessageQueueInitializer
    {
        private readonly IChannel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueInitializer"/> class using the provided RabbitMQ channel.
        /// </summary>
        /// <param name="channel">The RabbitMQ channel used to declare queues and exchanges.</param>
        public MessageQueueInitializer(IChannel channel)
        {
            _channel = channel;
        }

        /// <summary>
        /// Ensures the creation and binding of a main queue along with its Dead Letter Exchange (DLX) and Dead Letter Queue (DLQ).
        /// </summary>
        /// <param name="queueName">The name of the main queue to create and configure.</param>
        /// <param name="cancellationToken">A token used to cancel the asynchronous operations if needed.</param>
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


