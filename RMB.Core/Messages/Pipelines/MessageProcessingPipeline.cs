using FluentValidation;
using Microsoft.Extensions.Logging;
using RMB.Core.Messages.Validations;


namespace RMB.Core.Messages.Pipelines
{

    public static class MessageProcessingPipeline
    {
        public static IMessageMiddleware<T> Build<T>(
            IValidator<T> validator,
            Func<T, CancellationToken, Task> finalHandler,
            ILogger logger)
            where T : class
        {
            var validationMiddleware = new FluentValidationMiddleware<T>(validator, async (msg, ct) =>
            {
                await finalHandler(msg, ct);
                return true;
            }, logger);

            return new JsonDeserializationMiddleware<T>(
                validationMiddleware.InvokeAsync,
                logger
            );
        }
    }

}
