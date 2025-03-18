using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using System.Reflection;


namespace RMB.Core.Logs.Extensions
{
    /// <summary>
    /// Provides methods to configure logging for the Windows Event Viewer.
    /// This extension ensures logs are structured and contain Correlation IDs for traceability.
    /// </summary>
    public static class LoggingExtensions 
    {
        /// <summary>
        /// Configures Serilog to write logs to the Windows Event Viewer.
        /// Ensures that logs contain Correlation IDs and structured messages.
        /// </summary>
        /// <param name="loggingBuilder">The logging builder used to register logging providers.</param>
        /// <param name="logName">The name of the event log where logs will be written.</param>
        public static void AddCustomLogging(this ILoggingBuilder loggingBuilder, string logName)
        {
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? GetMainAssemblyName();

            EventLogManager.EnsureEventSourceExists(logName); 

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .Enrich.FromLogContext() // Permite que o contexto seja herdado
                .WriteTo.EventLog(
                    source: applicationName,
                    logName: logName,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    manageEventSource: false,
                    outputTemplate: "CorrelationId: {CorrelationId} - {Message}{NewLine}{Exception}"
                )
                .CreateLogger();
            
            // se retiraro comentário da linha de baixo os logs do param de aparecer no console
            //loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
            
            Log.Information($"Logging configurado para Event Viewer! ");
        }

        /// <summary>
        /// Retrieves the name of the main application assembly.
        /// Used to determine the default event log source name.
        /// </summary>
        /// <returns>The name of the main assembly, or null if it cannot be determined.</returns>
        private static string? GetMainAssemblyName()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.GetName().Name))
                .OrderByDescending(a => a.GetTypes().Length)
                .Select(a => a.GetName().Name)
                .FirstOrDefault();
        }
    }
}
