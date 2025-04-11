// Interfaces/IDeadLetterHandler.cs
using System.Threading;
using System.Threading.Tasks;

namespace RMB.Abstractions.Infrastructure.Messages.Interfaces
{
    /// <summary>
    /// Defines a contract for handling fallback scenarios for failed message processing.
    /// </summary>
    public interface IDeadLetterHandler
    {
        Task HandleAsync(string messageBody, string errorDetails, string messageType, string category, CancellationToken cancellationToken);
    }
}