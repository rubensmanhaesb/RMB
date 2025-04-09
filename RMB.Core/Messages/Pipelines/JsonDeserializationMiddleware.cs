using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace RMB.Core.Messages.Pipelines
{

    public class JsonDeserializationMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly Func<T, CancellationToken, Task<bool>> _next;
        private readonly ILogger _logger;

        public JsonDeserializationMiddleware(
            Func<T, CancellationToken, Task<bool>> next,
            ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

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
                _logger.LogError(ex, "Failed to deserialize message.");
                return false;
            }
        }
    }

}
