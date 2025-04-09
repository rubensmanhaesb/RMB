

namespace RMB.Core.Messages.Pipelines
{
    public interface IMessageMiddleware<T>
    {
        Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken);
    }

}
