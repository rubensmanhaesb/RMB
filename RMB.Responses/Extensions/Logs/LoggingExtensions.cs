using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using System.Reflection;


namespace RMB.Responses.Extensions.Logs
{
    public static class LoggingExtensions
    {
        public static void AddCustomLogging(this ILoggingBuilder loggingBuilder, string logName)
        {
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? GetMainAssemblyName();

            EventLogManager.EnsureEventSourceExists(logName); 

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .WriteTo.EventLog(
                    source: applicationName,
                    logName: logName,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    manageEventSource: false
                )
                .CreateLogger();

            // se retiraro comentário da linha de baixo os logs do param de aparecer no console
            //loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);

            Log.Information($"Logging configurado para Event Viewer! ");// [{MethodHelper.GetCurrentMethodName()}]");
        }

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
