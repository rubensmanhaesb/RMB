using System.Diagnostics;
using System.Reflection;

namespace RMB.Responses.Logs.Extensions.Logs
{
    /// <summary>
    /// Provides utility methods for retrieving the name of the caller method.
    /// </summary>
    public static class MethodHelper
    {
        /// <summary>
        /// Gets the name of the calling method, excluding methods from this helper class.
        /// </summary>
        /// <returns>A string containing the full name of the calling method.</returns>
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
