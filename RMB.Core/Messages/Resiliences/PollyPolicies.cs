using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;

namespace RMB.Core.Messages.Resiliences
{
    /// <summary>
    /// Provides resilience policies using Polly, including retry and circuit breaker mechanisms.
    /// </summary>
    public static class PollyPolicies
    {
        /// <summary>
        /// Creates a resilience policy that combines retry and circuit breaker strategies for operations returning a boolean.
        /// </summary>
        /// <param name="logger">Logger used to output diagnostic information.</param>
        /// <returns>An asynchronous resilience policy composed of retry and circuit breaker.</returns>
        public static AsyncPolicy<bool> CreateResiliencePolicy(ILogger logger)
        {
            // Trata exceções e resultados inválidos (false)
            var policyBuilder = Policy
                .Handle<Exception>()
                .OrResult<bool>(result => !result);

            var retryPolicy = policyBuilder
                .RetryAsync(3, onRetry: (resultado, tentativa, _) =>
                {
                    var erro = resultado.Exception?.Message ?? "Operação retornou falso.";
                    logger.LogWarning("Tentativa {Tentativa} falhou: {Erro}", tentativa, erro);
                });

            var circuitBreakerPolicy = policyBuilder
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (resultado, tempoDeEspera) =>
                    {
                        var motivo = resultado.Exception?.Message ?? "Operação retornou falso.";
                        logger.LogWarning("Circuito aberto por {Tempo} devido a: {Motivo}", tempoDeEspera, motivo);
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuito fechado novamente (reset).");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogInformation("Circuito em estado half-open: próxima chamada será um teste.");
                    });

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }
    }
}
