using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RMB.Abstractions.UseCases.Logs;
using RMB.Core.Logs.Mediator.Behavior;
using RMB.Core.Logs.Services;
using RMB.Core.Logs.Settings;
using RMB.Core.ValuesObjects.Logradouro;
using RMB.Core.ValuesObjects.Logradouro.Validations;

namespace RMB.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for registering core services in the dependency injection container.
    /// This includes logging, MediatR pipeline behaviors, and Correlation ID management.
    /// </summary>
    public static class RMBCoreExtensions
    {
        /// <summary>
        /// Registers core services required for the application.
        /// This method adds MediatR pipeline behaviors, an HTTP context accessor, Correlation ID management, 
        /// and logging settings.
        /// </summary>
        /// <param name="services">The dependency injection service collection.</param>
        /// <param name="configuration">The application configuration containing logging settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with the registered core services.</returns>
        public static IServiceCollection AddCoreExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            #region Logging
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddHttpContextAccessor();
            services.AddSingleton<ICorrelationIdProvider, CorrelationIdProviderService>();
            services.Configure<LoggingSettings>(configuration.GetSection("Logging"));

            #endregion Logging
            services.AddHttpClient<IViaCepService, ViaCepService>();
            services.AddScoped<EnderecoValidation>();


            return services;
        }
    }
}
