using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class PerformanceBehaviour<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IAppUserService _user;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            IAppUserService user)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _user = user;
        }

        public async Task<TResponse> Handle(
            TRequest request, CancellationToken
            cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var identifier = _user.UniqueIdentifier;
                var claimId = _user.ClaimID;

                _logger.LogWarning(
                    $"Application Long Running Request: {requestName} " +
                    $"({elapsedMilliseconds} milliseconds) {claimId} - {identifier}" +
                    $"{request}");
            }
            await Task.FromResult(0);
            return response;
        }
    }
}
