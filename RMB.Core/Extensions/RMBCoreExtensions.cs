using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RMB.Abstractions.UseCases.Logs;
using RMB.Core.Logs.Behavior;
using RMB.Core.Logs.Services;

namespace RMB.Core.Extensions
{
    /// <summary>
    /// Provides extension methods to register core services in the dependency injection container.
    /// </summary>
    public static class RMBCoreExtensions
    {
        /// <summary>
        /// Adds core services to the dependency injection container.
        /// This includes logging, MediatR pipeline behaviors, and Correlation ID management.
        /// </summary>
        /// <param name="services">The service collection to which the core services will be added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
        public static IServiceCollection AddCoreExtensions(this IServiceCollection services)
        {
            #region Logging
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddHttpContextAccessor();
            services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();
            #endregion Logging

            return services;
        }
    }
}
