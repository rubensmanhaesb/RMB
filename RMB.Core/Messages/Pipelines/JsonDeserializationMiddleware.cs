using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace RMB.Core.Messages.Pipelines
{
    /// <summary>
    /// Middleware responsible for deserializing the raw message body into a strongly typed object.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized message.</typeparam>
    public class JsonDeserializationMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly Func<T, CancellationToken, Task<bool>> _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDeserializationMiddleware{T}"/> class.
        /// </summary>
        /// <param name="next">The next middleware or handler in the pipeline.</param>
        /// <param name="logger">The logger used to log deserialization issues.</param>
        public JsonDeserializationMiddleware(
            Func<T, CancellationToken, Task<bool>> next,
            ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Attempts to deserialize the incoming message and passes it to the next middleware in the pipeline.
        /// </summary>
        /// <param name="body">The raw message body.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>
        /// Returns <c>true</c> if deserialization and next middleware execution succeed; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken)
        {
            string json;  
            try
            {
                json = Encoding.UTF8.GetString(body.Span);
                var obj = JsonConvert.DeserializeObject<T>(json);

                if (obj == null)
                {
                    _logger.LogWarning("Falha na desserialização. JSON: {Json}", json);
                    return false;
                }

                return await _next(obj, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na desserialização na messagem.");
                return false;
            }
        }
    }

}
