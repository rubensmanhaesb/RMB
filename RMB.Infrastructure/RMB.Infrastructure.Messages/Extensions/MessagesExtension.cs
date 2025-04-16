using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Messages.Events;
using RMB.Core.Messages.Events.BackgroundEventPublisher;
using RMB.Core.Messages.FailedMessages.Services;
using RMB.Core.Messages.Pipelines.Middlewares;
using RMB.Infrastructure.Messages.Consumers;
using RMB.Infrastructure.Messages.Producer;
using RMB.Infrastructure.Messages.Services;
using RMB.Infrastructure.Messages.Validations;


namespace RMB.Infrastructure.Messages.Extensions
{

    /// <summary>
    /// Provides extension methods for configuring message processing services.
    /// </summary>
    /// <remarks>
    /// This extension configures the complete email messaging pipeline including:
    /// - Message producers and consumers
    /// - Validation infrastructure
    /// - Processing middleware
    /// - Background services
    /// - Error handling and dead-letter queue processing
    /// - Success/failure event publishing
    /// </remarks>
    public static class MessagesExtension
    {
        /// <summary>
        /// Registers all services required for email message processing with RabbitMQ.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        /// <example>
        /// <code>
        /// services.AddMailMessages()
        ///         .AddRabbitMQConnection();
        /// </code>
        /// </example>
        public static IServiceCollection AddMailMessages(this IServiceCollection services)
        {

            services.AddTransient<IMessageProducer, MessageProducer>();
            services.AddTransient<MailService>();
            services.AddTransient<IValidator<EmailConfirmationMessage>, EmailConfirmationMessageValidator>();
            services.AddTransient(typeof(JsonDeserializationMiddleware<>));

            #region Background services

            services.AddHostedService<MailMessageConsumer>();
            services.AddHostedService<DlqMessageConsumer>();
            services.AddSingleton<IMessageErrorEventPublisher, MessageErrorEventPublisher>();
            
            #region Falhas no processamentodas mensagens
            // Message failure service (core)
            services.AddTransient<IMessageFailureService, MessageFailureService>();
            // Message failure repository (infrastructure)
            services.AddTransient<IMessageDeadLetterHandler, MessageDeadLetterHandler>();
            #endregion Falhas no processamentodas mensagens

            services.AddSingleton<IMessageSuccessEventPublisher, MessageSuccessEventPublisher>();

            #endregion Background services

            return services;
        }
    }
}
