using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Iris.Crosscutting.Domain.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {request}");

            await Task.CompletedTask;
            //TODO: CRIAR ROTINA PARA GRAVAÇÃO DE LOG APÓS A EXECUÇÃO DE UM COMMAND/QUERY
            //await _serviceBus.SendLog(request).ConfigureAwait(false);

            _logger.LogInformation($"Handled {request}");
        }
    }
}