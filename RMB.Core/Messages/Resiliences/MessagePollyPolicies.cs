using FluentValidation;
using Newtonsoft.Json;
using Polly;
using Serilog;


namespace RMB.Core.Messages.Resiliences
{
    /// <summary>
    /// Defines Polly-based resilience policies (Retry and Circuit Breaker) for message processing.
    /// </summary>
    public static class MessagePollyPolicies
    {

        /// <summary>
        /// Creates and configures a resilience policy combining retry and circuit breaker strategies.
        /// </summary>
        /// <returns>The composed asynchronous policy for handling resilience.</returns>
        public static AsyncPolicy<bool> CreateResiliencePolicy()
        {
            Log.Information("CreateResiliencePolicy iniciando...");

            var retryPolicy = Policy
                .Handle<Exception>(ex => DeveTratar(ex)) // só entra aqui se for exceção tratável
                .OrResult<bool>(result => false) // aceita também retorno false
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 2),
                    onRetry: (outcome, delay, retryAttempt, context) =>
                    {
                        // Executado em qualquer caso (exceção ou retorno false)
                        if (outcome.Exception is not null)
                        {
                            var tratar = DeveTratar(outcome.Exception);
                            if (!tratar)
                            {
                                Log.Warning("Retry ignorado para exceção não tratável: {Tipo}", outcome.Exception.GetType().Name);
                                return;
                            }

                            Log.Warning("Retry #{RetryAttempt} - Exception: {Message}", retryAttempt, outcome.Exception.Message);
                        }
                        else
                        {
                            // false sem exceção (não faz retry nesse caso!)
                            Log.Warning("Resultado false sem exceção - Retry NÃO será realizado.");
                        }
                    });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>(ex => DeveTratar(ex))
                .OrResult<bool>(result => false)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, delay) =>
                    {
                        var msg = outcome.Exception?.Message ?? "Retorno false.";
                        Log.Error("CIRCUIT Polly - Aberto por {Delay}s: {Msg}", delay.TotalSeconds, msg);
                    },
                    onReset: () => Log.Information("CIRCUIT Polly - Resetado"),
                    onHalfOpen: () => Log.Information("CIRCUIT Polly - Half-Open")
                );

            Log.Information("CreateResiliencePolicy finalizado.");
            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        /// <summary>
        /// Determines whether the given exception should be handled by the policies.
        /// </summary>
        private static bool DeveTratar(Exception ex)
        {
            var realEx = DescompactarExcecao(ex);
            var tipo = realEx.GetType().FullName;
            Log.Information("DeveTratar ---- Polly[{}] - Exceção avaliada: {Tipo}",  tipo);
            Log.Information("DeveTratar ---- realEx[{}] ", realEx.GetType().ToString());
            return realEx is not ValidationException && realEx is not JsonException;
        }

        /// <summary>
        /// Recursively unwraps aggregate or inner exceptions to retrieve the actual exception.
        /// </summary>
        private static Exception DescompactarExcecao(Exception ex)
        {
            if (ex is AggregateException agg && agg.InnerExceptions.Count == 1)
                return DescompactarExcecao(agg.InnerExceptions[0]);

            return ex.InnerException != null ? DescompactarExcecao(ex.InnerException) : ex;
        }
    }
}
