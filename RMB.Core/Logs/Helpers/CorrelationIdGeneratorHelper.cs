namespace RMB.Core.Logs.Helpers
{
    /// <summary>
    /// Utility class responsible for generating unique Correlation IDs.
    /// </summary>
    public static class CorrelationIdGeneratorHelper
    {
        /// <summary>
        /// Generates a new unique Correlation ID in a shortened format (without hyphens).
        /// </summary>
        /// <returns>A string representing the newly generated Correlation ID.</returns>
        public static string GenerateNewCorrelationId()
        {
            return Guid.NewGuid().ToString("N"); // Gera um GUID curto (sem hífens)
        }
    }
}
