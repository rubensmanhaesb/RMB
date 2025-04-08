using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages;
using RMB.Infrastructure.Messages.Helpers;
using System.Text;

namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Background service responsible for consuming email confirmation messages from RabbitMQ.
    /// </summary>
    public class MailMessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MailHelper _mailHelper;
        private readonly ILogger<MailMessageConsumer> _logger;

        private IConnection? _connection;
        private IChannel? _channel;

        public MailMessageConsumer(
            IConfiguration configuration,
            MailHelper mailHelper,
            ILogger<MailMessageConsumer> logger)
        {
            _configuration = configuration;
            _mailHelper = mailHelper;
            _logger = logger;
        }

        /// <summary>
        /// Initializes RabbitMQ connection and declares the queue on service start.
        /// </summary>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQSettings:Host"],
                Port = int.Parse(_configuration["RabbitMQSettings:Port"]),
                UserName = _configuration["RabbitMQSettings:Username"],
                Password = _configuration["RabbitMQSettings:Password"],
                VirtualHost = _configuration["RabbitMQSettings:VHost"]
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _configuration["RabbitMQSettings:Queue"],
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Starts consuming messages from the RabbitMQ queue.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null)
            {
                _logger.LogError("Channel is not initialized");
                return;
            }

            var consumer = new CustomAsyncConsumer(_channel, _mailHelper, _logger);

            await _channel.BasicConsumeAsync(
                queue: _configuration["RabbitMQSettings:Queue"],
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            // Keeps the background service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Gracefully closes the RabbitMQ channel and connection.
        /// </summary>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
            {
                await _channel.CloseAsync(cancellationToken: cancellationToken);
            }

            if (_connection is not null)
            {
                await _connection.CloseAsync(cancellationToken: cancellationToken);
            }

            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Disposes RabbitMQ resources.
        /// </summary>
        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Custom asynchronous consumer implementation for processing incoming messages.
        /// </summary>
        private class CustomAsyncConsumer : AsyncDefaultBasicConsumer
        {
            private readonly MailHelper _mailHelper;
            private readonly ILogger<MailMessageConsumer> _logger;

            public CustomAsyncConsumer(
                IChannel channel,
                MailHelper mailHelper,
                ILogger<MailMessageConsumer> logger) : base(channel)
            {
                _mailHelper = mailHelper;
                _logger = logger;
            }

            /// <summary>
            /// Handles incoming messages from the queue, deserializes the payload and triggers email sending.
            /// </summary>
            public override async Task HandleBasicDeliverAsync(
                string consumerTag,
                ulong deliveryTag,
                bool redelivered,
                string exchange,
                string routingKey,
                IReadOnlyBasicProperties properties,
                ReadOnlyMemory<byte> body,
                CancellationToken cancellationToken = default)
            {
                try
                {
                    var message = Encoding.UTF8.GetString(body.Span);
                    var emailConfirmationMessage = JsonConvert.DeserializeObject<EmailConfirmationMessage>(message);

                    if (emailConfirmationMessage != null)
                    {
                        await _mailHelper.SendAsync(emailConfirmationMessage);
                    }

                    await Channel.BasicAckAsync(
                        deliveryTag: deliveryTag,
                        multiple: false,
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");

                    await Channel.BasicNackAsync(
                        deliveryTag: deliveryTag,
                        multiple: false,
                        requeue: false,
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}
