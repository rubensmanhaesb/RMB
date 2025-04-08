using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages;
using System.Text;

namespace RMB.Infrastructure.Messages.Producer
{
    public class MailMessageProducer : IMailMessageProducer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;

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

        public async Task PublishEmailConfirmationAsync(EmailConfirmationMessage emailConfirmationMessage)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonConvert.SerializeObject(emailConfirmationMessage);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true
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