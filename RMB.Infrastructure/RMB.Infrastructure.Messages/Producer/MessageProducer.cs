using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Messages.Queues;
using System.Text;

namespace RMB.Infrastructure.Messages.Producer
{
    /// <summary>
    /// Publishes email confirmation messages to a RabbitMQ queue.
    /// </summary>
    public class MessageProducer : IMessageProducer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;

        /// <summary>
        /// Initializes the producer with RabbitMQ settings from configuration.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        public MessageProducer(IConfiguration configuration)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQSettings:Host"],
                Port = int.Parse(configuration["RabbitMQSettings:Port"]),
                UserName = configuration["RabbitMQSettings:Username"],
                Password = configuration["RabbitMQSettings:Password"],
                VirtualHost = configuration["RabbitMQSettings:VHost"]
            };

            _queueName = configuration["RabbitMQSettings:Queue"];
        }

        /// <summary>
        /// Publishes an email confirmation message to the configured RabbitMQ queue.
        /// Before publishing, it ensures that the queue and its Dead Letter Queue (DLQ) are properly declared.
        /// </summary>
        /// <param name="emailConfirmationMessage">The email confirmation message to publish.</param>
        public async Task PublishEmailConfirmationAsync(EmailConfirmationMessage emailConfirmationMessage)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var initializer = new MessageQueueInitializer(channel);
            await initializer.EnsureQueueWithDeadLetterAsync(_queueName, CancellationToken.None);

            var json = JsonConvert.SerializeObject(emailConfirmationMessage);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true // Ensures message will survive broker restarts
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _queueName,
                basicProperties: properties,
                body: body,
                mandatory: false);
        }
    }
}
