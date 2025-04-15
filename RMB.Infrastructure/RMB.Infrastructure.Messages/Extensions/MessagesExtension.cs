using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
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
    /// Extension class for configuring all services related to email message handling and background processing.
    /// </summary>
    public static class MessagesExtension
    {
        /// <summary>
        /// Registers all required components for sending and processing email confirmation messages via RabbitMQ.
        /// This includes producers, consumers, validation, middleware, and background services.
        /// </summary>
        /// <param name="services">The service collection to register components into.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddMailMessages(this IServiceCollection services)
        {
            // Producer
            //producer
            services.AddTransient<IMessageProducer, MessageProducer>();
            //service
            services.AddTransient<MailService>();
            //validation
            services.AddTransient<IValidator<EmailConfirmationMessage>, EmailConfirmationMessageValidator>();
            // Registro do pipeline, 
            services.AddTransient(typeof(JsonDeserializationMiddleware<>));

            #region Background services
            // Background consumer
            services.AddHostedService<MailMessageConsumer>();
            // Background consumer for dead letter queue
            services.AddHostedService<DlqMessageConsumer>();
            // Background consumer for failed messages
            services.AddSingleton<IMessageBackgroundEventPublisher, MessageBackgroundEventPublisher>();
            // Background consumer for failed messages
            //services.AddScoped<IMessageBackgroundExceptionHandler, ApiBackgroundExceptionHandler>();
            
            #region Falhas no processamentodas mensagens
            // Message failure service (core)
            services.AddTransient<IMessageFailureService, MessageFailureService>();
            // Message failure repository (infrastructure)
            services.AddTransient<IMessageDeadLetterHandler, MessageDeadLetterHandler>();
            #endregion Falhas no processamentodas mensagens

            #endregion Background services

            return services;
        }
    }
}
