
namespace RMB.Core.Logs.Services
{
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Enriches Serilog logs by adding the caller's class name and method name.
    /// </summary>
    internal class CallerEnricher : ILogEventEnricher
    {
        private readonly ICallerInfoProvider _callerInfoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerEnricher"/> class.
        /// </summary>
        /// <param name="callerInfoProvider">Service to retrieve caller information.</param>
        public CallerEnricher(ICallerInfoProvider callerInfoProvider)
        {
            _callerInfoProvider = callerInfoProvider;
        }

        /// <summary>
        /// Enriches the log event with caller class and method names.
        /// </summary>
        /// <param name="logEvent">The log event being processed.</param>
        /// <param name="propertyFactory">Factory for creating log event properties.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var (className, methodName) = _callerInfoProvider.GetCallerInfo();

            if (className != null)
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CallerClassName", className));

            if (methodName != null)
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CallerMethodName", methodName));
        }
    }

}