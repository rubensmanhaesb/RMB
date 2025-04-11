using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Messages.FailedMessages.Services;
using RMB.Core.Messages.Pipelines;
using RMB.Core.Messages.Validations;
using RMB.Infrastructure.Messages.Consumers;
using RMB.Infrastructure.Messages.Helpers;
using RMB.Infrastructure.Messages.Producer;
using RMB.Infrastructure.Messages.Validations;


namespace RMB.Infrastructure.Messages.Extensions
{
    public static class MailMessagesExtension
    {
        public static IServiceCollection AddMailMessages(this IServiceCollection services)
        {
            // Producer
            services.AddTransient<IMailMessageProducer, MailMessageProducer>();

            // Helper de envio de e-mails
            services.AddTransient<MailHelper>();

            // FluentValidation para o DTO
            services.AddTransient<IValidator<EmailConfirmationMessage>, EmailConfirmationMessageValidator>();

            // Registro do pipeline,
            services.AddTransient(typeof(FluentValidationMiddleware<>));
            services.AddTransient(typeof(JsonDeserializationMiddleware<>));

            // Background consumer
            services.AddHostedService<MailMessageConsumer>();
            
            // Message failure service (core)
            services.AddTransient<IMessageFailureService, MessageFailureService>();

            services.AddTransient<IDeadLetterHandler, DeadLetterHandler>();

            return services;
        }
    }
}
