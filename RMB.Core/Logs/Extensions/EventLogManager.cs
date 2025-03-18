using System.Diagnostics;

namespace RMB.Core.Logs.Extensions
{
    /// <summary>
    /// Provides utilities for managing Windows Event Viewer logs.
    /// This class ensures that the necessary Event Sources exist before logging.
    /// </summary>
    public static class EventLogManager
    {
        /// <summary>
        /// Ensures that the specified Event Source exists in the Windows Event Viewer.
        /// If the source does not exist, it attempts to create it.
        /// </summary>
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
