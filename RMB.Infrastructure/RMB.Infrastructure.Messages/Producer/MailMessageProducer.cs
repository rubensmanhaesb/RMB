using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages;
using System.Text;

namespace RMB.Infrastructure.Messages.Producer
{
    /// <summary>
    /// Publishes email confirmation messages to a RabbitMQ queue.
    /// </summary>
    public class MailMessageProducer : IMailMessageProducer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;

        /// <summary>
        /// Initializes the producer with RabbitMQ settings from configuration.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        public MailMessageProducer(IConfiguration configuration)
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
        /// </summary>
        /// <param name="emailConfirmationMessage">The message to be published.</param>
        public async Task PublishEmailConfirmationAsync(EmailConfirmationMessage emailConfirmationMessage)
        {
            // Establish a connection and open a channel
            await using var connection = await _connectionFactory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            // Declare the queue to ensure it exists
            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Serialize the message object to JSON
            var json = JsonConvert.SerializeObject(emailConfirmationMessage);
            var body = Encoding.UTF8.GetBytes(json);

            // Set message properties
            var properties = new BasicProperties
            {
                Persistent = true // Ensures message will survive broker restarts
            };

            // Publish the message to the queue
            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _queueName,
                basicProperties: properties,
                body: body,
                mandatory: false);
        }
    }
}
