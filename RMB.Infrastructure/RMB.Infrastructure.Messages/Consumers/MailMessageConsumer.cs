using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages;
using RMB.Infrastructure.Messages.Helpers;
using FluentValidation;

namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Background service that listens to RabbitMQ and processes email confirmation messages.
    /// </summary>
    public class MailMessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MailHelper _mailHelper;
        private readonly ILogger<MailMessageConsumer> _logger;
        private readonly IValidator<EmailConfirmationMessage> _validator;

        private IConnection? _connection;
        private IChannel? _channel;

        /// <summary>
        /// Constructor that receives dependencies via DI.
        /// </summary>
        public MailMessageConsumer(
            IConfiguration configuration,
            MailHelper mailHelper,
            IValidator<EmailConfirmationMessage> validator,
            ILogger<MailMessageConsumer> logger)
        {
            _configuration = configuration;
            _mailHelper = mailHelper;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// Initializes RabbitMQ connection and declares the queue.
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
        /// Starts the background process that continuously listens and processes messages from RabbitMQ.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null)
            {
                _logger.LogError("Canal não inicializado.");
                return;
            }

            var consumer = new CustomAsyncConsumer(_channel, _mailHelper, _validator, _logger);

            await _channel.BasicConsumeAsync(
                queue: _configuration["RabbitMQSettings:Queue"],
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Gracefully stops the service and closes RabbitMQ resources.
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
        /// Disposes the RabbitMQ connection and channel.
        /// </summary>
        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }

    }
}
