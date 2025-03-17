using System.Diagnostics;
using System.Reflection;

namespace RMB.Responses.Extensions.Logs
{
    public static class MethodHelper
    {
        public static string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace();
            foreach (var frame in stackTrace.GetFrames() ?? Array.Empty<StackFrame>())
            {
                var method = frame.GetMethod();
                if (method?.DeclaringType != typeof(MethodHelper)) // Evita capturar métodos internos da classe de log
                {
                    return $"{method?.DeclaringType?.FullName}.{method?.Name}";
                }
            }
            return "Método Desconhecido";
        }
    }
}
