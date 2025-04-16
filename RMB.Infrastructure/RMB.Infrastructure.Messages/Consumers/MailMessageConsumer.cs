using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using RabbitMQ.Client;
using FluentValidation;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Infrastructure.Messages.Services;
using RMB.Core.Messages.Queues;

namespace RMB.Infrastructure.Messages.Consumers
{
    /// <summary>
    /// Background service that consumes email confirmation messages from RabbitMQ and processes them through a pipeline.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service implements:
    /// - RabbitMQ connection management
    /// - Queue initialization with dead-letter configuration
    /// - Message consumption with manual acknowledgment
    /// - Graceful shutdown procedures
    /// </para>
    /// <para>
    /// The processing pipeline includes validation, email sending, and error handling with dead-letter routing.
    /// </para>
    /// </remarks>
    public class MailMessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MailService _mailService;
        private readonly IValidator<EmailConfirmationMessage> _validator;
        private readonly IMessageDeadLetterHandler _deadLetterHandler;
        private readonly IMessageErrorEventPublisher _messageErrorEventPublisher;
        private readonly IMessageSuccessEventPublisher _messageSuccessEventPublisher;


        private IConnection? _connection;
        private IChannel? _channel;

        /// <summary>
        /// Initializes a new instance of the email message consumer service.
        /// </summary>
        /// <param name="configuration">Application configuration containing RabbitMQ settings.</param>
        /// <param name="mailService">Service responsible for sending email confirmations.</param>
        /// <param name="validator">Validator for email confirmation messages.</param>
        /// <param name="deadLetterHandler">Handler for failed messages that go to dead-letter queue.</param>
        /// <param name="messageErrorEventPublisher">Publisher for error events.</param>
        /// <param name="messageSuccessEventPublisher">Publisher for success events.</param>
        /// <exception cref="ArgumentNullException">Thrown when any required dependency is null.</exception>
        public MailMessageConsumer(
            IConfiguration configuration,
            MailService mailService,
            IValidator<EmailConfirmationMessage> validator,
            IMessageDeadLetterHandler deadLetterHandler,
            IMessageErrorEventPublisher messageErrorEventPublisher,
            IMessageSuccessEventPublisher messageSuccessEventPublisher)
        {
            _configuration = configuration;
            _mailService = mailService;
            _validator = validator;
            _deadLetterHandler = deadLetterHandler;
            _messageErrorEventPublisher = messageErrorEventPublisher;
            _messageSuccessEventPublisher = messageSuccessEventPublisher;
        }

        /// <summary>
        /// Establishes RabbitMQ connection and initializes queues during service startup.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required RabbitMQ configuration is missing.</exception>
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

            var initializer = new MessageQueueInitializer(_channel);
            await initializer.EnsureQueueWithDeadLetterAsync(_configuration["RabbitMQSettings:Queue"], cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Main processing loop that consumes messages from the queue.
        /// </summary>
        /// <param name="stoppingToken">Cancellation token for service shutdown.</param>
        /// <returns>Task representing the long-running operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null)
            {
                Log.Error("Canal não inicializado.");
                return;
            }

            var consumer = new EmailConfirmationConsumer(_channel, _mailService, _validator, _deadLetterHandler, _messageErrorEventPublisher, _messageSuccessEventPublisher);

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
        /// Gracefully shuts down the RabbitMQ connection during service stopping.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
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
        /// Releases all managed RabbitMQ resources.
        /// </summary>
        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }

    }
}
