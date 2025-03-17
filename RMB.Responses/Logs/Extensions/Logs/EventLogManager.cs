using System.Diagnostics;

namespace RMB.Responses.Logs.Extensions.Logs
{
    /// <summary>
    /// Provides utilities for managing Windows Event Viewer logs.
    /// </summary>
    public static class EventLogManager
    {
        /// <summary>
        /// Ensures the specified Event Source exists in the Windows Event Viewer.
        /// </summary>
        /// <param name="sourceName">The name of the Event Source to be created if it does not exist.</param>
        /// <param name="logName">The name of the Event Log where events will be recorded.</param>
        public static void EnsureEventSourceExists(string logName)
        {
            if (!EventLog.SourceExists(logName))
            {
                try
                {
                    var sourceData = new EventSourceCreationData(logName, logName);
                    EventLog.CreateEventSource(sourceData);

                    Console.WriteLine($"Fonte de log '{sourceData}' criada no log '{logName}'.");
                }
                catch (System.Security.SecurityException)
                {
                    Console.WriteLine($"Sem permissão para criar a fonte de log '{logName}'. Execute como Administrador.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado ao criar a fonte de log: {ex.Message}");
                }
            }
        }
    }
}
