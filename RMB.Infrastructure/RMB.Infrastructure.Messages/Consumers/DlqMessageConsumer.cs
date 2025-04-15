using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Messages.Pipelines;
using RMB.Core.Messages.Resiliences;
using RMB.Infrastructure.Messages.Services;
using Serilog;

namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Background service that consumes the Dead Letter Queue (DLQ) and attempts to reprocess failed messages.
    /// </summary>
    public class DlqMessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MailService _mailHelper;
        private readonly IValidator<EmailConfirmationMessage> _validator;
        private readonly IMessageDeadLetterHandler _failureHandler;
        private readonly IMessageBackgroundEventPublisher _eventPublisher;

        private IConnection? _connection;
        private IChannel? _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DlqMessageConsumer"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="mailHelper">Service responsible for sending emails.</param>
        /// <param name="validator">Validator for email confirmation messages.</param>
        /// <param name="failureHandler">Handler responsible for managing failed messages.</param>
        /// <param name="eventPublisher">Service responsible for publishing background errors.</param>
        public DlqMessageConsumer(
            IConfiguration configuration,
            MailService mailHelper,
            IValidator<EmailConfirmationMessage> validator,
            IMessageDeadLetterHandler failureHandler,
            IMessageBackgroundEventPublisher eventPublisher)
        {
            _configuration = configuration;
            _mailHelper = mailHelper;
            _validator = validator;
            _failureHandler = failureHandler;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Establishes the RabbitMQ connection and initializes the channel.
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

            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Starts the DLQ consumer and continuously listens for failed messages to retry processing.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null)
            {
                Log.Error("Canal não inicializado.");
                return;
            }

            var dlqName = _configuration["RabbitMQSettings:Queue"] + ".dlq";

            var pipeline = MessageProcessingPipeline.Build(
                _validator,
                async (msg, ct) => await _mailHelper.SendAsync(msg),
                _failureHandler,
                _eventPublisher 
            );


            var consumer = new DlqEmailConsumer(_channel, pipeline.InvokeAsync, MessagePollyPolicies.CreateResiliencePolicy());

            await _channel.BasicConsumeAsync(
                queue: dlqName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Closes RabbitMQ channel and connection gracefully when the service is stopping.
        /// </summary>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken);

            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Releases RabbitMQ resources.
        /// </summary>
        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }


}
