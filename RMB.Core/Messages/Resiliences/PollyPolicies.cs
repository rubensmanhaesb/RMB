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
        /// Creates a resilience policy that combines exponential retry and circuit breaker for boolean-returning operations.
        /// </summary>
        /// <param name="logger">Logger used to register diagnostic information.</param>
        /// <returns>An asynchronous composite policy to handle failures in a resilient way.</returns>
        public static AsyncPolicy<bool> CreateResiliencePolicy(ILogger logger)
        {
            // Trata exceções e resultados inválidos (false)
            var policyBuilder = Policy
                .Handle<Exception>()
                .OrResult<bool>(result => !result);

            // Retry com backoff progressivo (2s, 4s, 6s)
            var retryPolicy = policyBuilder
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 2),
                    onRetry: (resultado, tempoDeEspera, tentativa, contexto) =>
                    {
                        var erro = resultado.Exception?.Message ?? "Operação retornou false.";
                        logger.LogWarning("Tentativa {Tentativa} falhou: {Erro}. Nova tentativa em {Espera}s.",
                            tentativa, erro, tempoDeEspera.TotalSeconds);
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
