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
    /// Background service that consumes messages from the Dead Letter Queue (DLQ) and attempts reprocessing.
    /// </summary>
    /// <remarks>
    /// This service provides resilient message reprocessing with:
    /// - Automatic connection recovery
    /// - Retry policies via Polly
    /// - Full processing pipeline execution
    /// - Proper resource cleanup
    /// </remarks>
    public class DlqMessageConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MailService _mailService;
        private readonly IValidator<EmailConfirmationMessage> _validator;
        private readonly IMessageDeadLetterHandler _failureHandler;
        private readonly IMessageErrorEventPublisher _messageBackgroundEventPublisher;
        private readonly IMessageSuccessEventPublisher _messageSuccessEventPublisher;

        private IConnection? _connection;
        private IChannel? _channel;

        /// <summary>
        /// Initializes a new instance of the DLQ consumer service.
        /// </summary>
        /// <param name="configuration">Application configuration for RabbitMQ settings.</param>
        /// <param name="mailHelper">Email service for processing email confirmation messages.</param>
        /// <param name="validator">Validator for email confirmation message content.</param>
        /// <param name="failureHandler">Handler for permanently failed messages.</param>
        /// <param name="messageBackgroundEventPublisher">Publisher for error events.</param>
        /// <param name="messageSuccessEventPublisher">Publisher for success events.</param>
        public DlqMessageConsumer(
            IConfiguration configuration,
            MailService mailHelper,
            IValidator<EmailConfirmationMessage> validator,
            IMessageDeadLetterHandler failureHandler,
            IMessageErrorEventPublisher messageBackgroundEventPublisher,
            IMessageSuccessEventPublisher messageSuccessEventPublisher
            )
        {
            _configuration = configuration;
            _mailService = mailHelper;
            _validator = validator;
            _failureHandler = failureHandler;
            _messageBackgroundEventPublisher = messageBackgroundEventPublisher;
            _messageSuccessEventPublisher = messageSuccessEventPublisher;
        }

        /// <summary>
        /// Establishes RabbitMQ connection and channel during service startup.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if RabbitMQ configuration is missing.</exception>
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
        /// Main processing loop that consumes messages from DLQ.
        /// </summary>
        /// <param name="stoppingToken">Cancellation token for service shutdown.</param>
        /// <returns>Task representing the long-running operation.</returns>
        /// <remarks>
        /// <para>
        /// The processing flow:
        /// 1. Creates complete processing pipeline
        /// 2. Configures resilient consumer with Polly policies
        /// 3. Starts consuming from DLQ
        /// 4. Runs until service shutdown
        /// </para>
        /// <para>
        /// Messages are manually acknowledged only after successful processing.
        /// </para>
        /// </remarks>
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
                async (msg, ct) => await _mailService.SendAsync(msg),
                _failureHandler,
                _messageBackgroundEventPublisher,
                _messageSuccessEventPublisher
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
        /// Gracefully shuts down RabbitMQ connections.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken);

            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken);

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
