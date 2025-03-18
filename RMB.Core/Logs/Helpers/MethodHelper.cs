using System.Diagnostics;

namespace RMB.Core.Logs.Helpers
{
    /// <summary>
    /// Provides utility methods for retrieving the name of the caller method.
    /// Useful for logging and debugging to track the execution flow.
    /// </summary>
    public static class MethodHelper
    {
        /// <summary>
        /// Retrieves the name of the calling method, excluding methods from this helper class.
        /// Ensures that the returned method name is from the actual application code.
        /// </summary>
        /// <returns>A string containing the full name (namespace and method) of the calling method.</returns>
        public static string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace();
            foreach (var frame in stackTrace.GetFrames() ?? Array.Empty<StackFrame>())
            {
                var method = frame.GetMethod();
                if (method?.DeclaringType != typeof(MethodHelper)) 
                {
                    return $"{method?.DeclaringType?.FullName}.{method?.Name}";
                }
            }
            return "Método Desconhecido";
        }
    }
}
