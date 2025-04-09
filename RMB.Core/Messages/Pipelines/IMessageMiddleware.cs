namespace RMB.Core.Messages.Pipelines
{
    /// <summary>
    /// Interface representing a middleware component in a message processing pipeline.
    /// </summary>
    /// <typeparam name="T">The type of message being processed.</typeparam>
    public interface IMessageMiddleware<T>
    {
        /// <summary>
        /// Invokes the middleware logic using the message body and cancellation token.
        /// </summary>
        /// <param name="body">The raw message body as a byte array (memory buffer).</param>
        /// <param name="cancellationToken">Token used to propagate cancellation requests.</param>
        /// <returns>True if the middleware handled the message successfully; otherwise, false.</returns>
        Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken);
    }
}
