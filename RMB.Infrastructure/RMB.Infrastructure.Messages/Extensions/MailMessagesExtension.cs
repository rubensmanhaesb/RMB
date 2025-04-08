using Microsoft.Extensions.DependencyInjection;
using RMB.Abstractions.Infrastructure.Messages;
using RMB.Infrastructure.Messages.Consumers;
using RMB.Infrastructure.Messages.Helpers;
using RMB.Infrastructure.Messages.Producer;

namespace RMB.Infrastructure.Messages.Extensions
{
    public static class MailMessagesExtension
    {
        public static IServiceCollection AddMailMessages(this IServiceCollection services)
        {
            services.AddTransient<IMailMessageProducer, MailMessageProducer>();
            services.AddTransient<MailHelper>();

            services.AddHostedService<MailMessageConsumer>();

            return services;
        }
    }
}
