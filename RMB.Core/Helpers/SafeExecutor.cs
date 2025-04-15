using Microsoft.Extensions.Logging;
using Serilog;

namespace RMB.Core.Helpers
{
    public static class SafeExecutor
    {
        public static Task ExecuteSafeAsync(Func<Task> action, string context)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Erro ao executar com segurança: {Context}", context);
                }
            });
        }

        public static Task ExecuteSafeAsync(Action action, string context)
        {
            return Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Erro ao executar com segurança: {Context}", context);
                }
            });
        }
    }
}
