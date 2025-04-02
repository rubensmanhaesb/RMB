using System.Diagnostics;

namespace RMB.Core.Logs.Services
{
    /// <summary>
    /// Provides caller information using StackTrace.
    /// </summary>
    internal class StackTraceCallerInfoProviderService : ICallerInfoProvider
    {
        private static readonly string[] _frameworkNamespaces =
        {
            "System.",
            "Microsoft.",
            "Serilog.",
            "FluentValidation.",
            "MediatR.",
            "Swashbuckle.",
            "Newtonsoft.",
            "EntityFrameworkCore.",
            "Polly.",
            "TaskAwaiter",
            "ExecutionContext",
            "AsyncStateMachineBox",
            "Internal.",
            "AsyncTaskMethodBuilder"
        };

        /// <summary>
        /// Retrieves the caller's class and method name using StackTrace, ignoring internal .NET methods.
        /// </summary>
        /// <returns>A tuple containing the caller's class and method name.</returns>
        public (string ClassName, string MethodName) GetCallerInfo()
        {
            var stackTrace = new StackTrace(skipFrames: 3, fNeedFileInfo: true);

            foreach (var frame in stackTrace.GetFrames() ?? Array.Empty<StackFrame>())
            {
                var method = frame.GetMethod();
                if (method == null) continue;

                var declaringType = method.DeclaringType;
                if (declaringType == null) continue;

                string className = declaringType.Name;
                string methodName = method.Name;
                string fullName = declaringType.FullName ?? string.Empty;

                // Ignorar métodos internos do .NET e bibliotecas externas
                if (_frameworkNamespaces.Any(ns => fullName.StartsWith(ns)) ||
                    methodName.Contains("MoveNext") ||
                    methodName.Contains("ExecutionContextCallback") ||
                    methodName.Contains("RunInternal") ||
                    methodName.Contains("AsyncStateMachine") ||
                    methodName.Contains("AsyncTaskMethodBuilder"))
                {
                    continue;
                }

                // Se chegou aqui, encontrou um método válido
                return (className, methodName);
            }

            return ("Unknown", "Unknown");
        }
    }
}