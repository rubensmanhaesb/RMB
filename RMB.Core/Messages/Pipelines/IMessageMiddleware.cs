namespace RMB.Core.Messages.Pipelines
{
    /// <summary>
    /// Represents a middleware component in a message processing pipeline.
    /// </summary>
    /// <typeparam name="T">The type of the message being processed.</typeparam>
    public interface IMessageMiddleware<T>
    {
        /// <summary>
        /// Executes the middleware logic on the provided message body.
        /// </summary>
        /// <param name="body">The raw message content as a memory buffer.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that returns true if the message was processed successfully; otherwise, false.
        /// </returns>
        Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken);
    }
}
