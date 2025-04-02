using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace RMB.Core.Logs.Lifecycle
{
    /// <summary>
    /// Manages application lifecycle events by logging when the application starts and stops.
    /// This class ensures that key application lifecycle events are captured in the logs
    /// for monitoring and debugging purposes.
    /// </summary>
    public static class AppLifecycleService
    {
        /// <summary>
        /// Configures event handlers to log application lifecycle events such as startup and shutdown.
        /// </summary>
        /// <param name="app">The WebApplication instance.</param>
        public static void ConfigureApplicationLifetime(WebApplication app)
        {
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

            Log.Information("Aplicação CRM.API foi iniciada.");

            lifetime.ApplicationStopping.Register(() =>
            {
                Log.Warning("Aplicação CRM.API está sendo encerrada...");
            });

            lifetime.ApplicationStopped.Register(() =>
            {
                Log.Fatal("Aplicação CRM.API foi encerrada");
            });

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                Log.Fatal("Aplicação encerrada pelo sistema ou pelo usuário");
            };
        }
    }
}
