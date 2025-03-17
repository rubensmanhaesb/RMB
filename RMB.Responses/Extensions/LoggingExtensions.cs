using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;

namespace RMB.Responses.Extensions
{
    /// <summary>
    /// Extension class for custom logging configuration using Serilog and Windows Event Viewer.
    /// Allows logging to both the console and the Event Viewer, while ignoring automatic framework logs.
    /// </summary>

    public static class LoggingExtensions
    {
        public static void AddCustomLogging1(this ILoggingBuilder loggingBuilder, string logName )
        {

            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? GetMainAssemblyName();
            
            
            if (!EventLog.SourceExists(logName))
            {
                try
                {
                    //  Garante que o log será criado no local correto (Logs de Aplicativos e Serviços)
                    var sourceData = new EventSourceCreationData(logName, logName);
                    EventLog.CreateEventSource(sourceData);

                    Console.WriteLine($"Fonte de log '{logName}' criada no Event Viewer.");
                }
                catch (System.Security.SecurityException)
                {
                    Console.WriteLine($"Sem permissão para criar a fonte de log '{applicationName}'. Execute como Administrador.");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado ao criar a fonte de log: {ex.Message}");
                    return;
                }
            }

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Ignora logs do framework
            .MinimumLevel.Override("System", LogEventLevel.Warning)    // Ignora logs do sistema
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

            Log.Information("Logging configurado para Event Viewer!");
        }

        /// <summary>
        /// Get the name of the Assembly in the main project.
        /// </summary>
        private static string? GetMainAssemblyName()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.GetName().Name)) // Ignora assemblies dinâmicos
                .OrderByDescending(a => a.GetTypes().Length) // Pega o assembly com mais tipos, normalmente o projeto principal
                .Select(a => a.GetName().Name)
                .FirstOrDefault();
        }
        public static string GetCurrentMethodName()
        {
            var method = new StackTrace().GetFrame(1)?.GetMethod();
            return $"{method?.DeclaringType?.FullName}.{method?.Name}";
        }
    }
}
