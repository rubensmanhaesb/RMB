using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMB.Abstractions.UseCases.Logs
{
    /// <summary>
    /// Interface for managing the Correlation ID.
    /// </summary>
    public interface ICorrelationIdProvider
    {
        /// <summary>
        /// Retrieves or generates a Correlation ID for the current request.
        /// </summary>
        string GetCorrelationId(HttpContext context);

        /// <summary>
        /// Ensures the Correlation ID is included in the response headers.
        /// </summary>
        void AddCorrelationIdToResponse(HttpContext context);
    }
}
