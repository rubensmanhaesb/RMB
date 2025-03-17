using System;
using System.Diagnostics;

namespace RMB.Responses.Extensions.Logs
{
    public static class EventLogManager
    {
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
