using Infrastructure.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class RequestPerformanceBehaviour<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IAuthService _authService;
        private readonly IIdentityService _identityService;

        public RequestPerformanceBehaviour(
            ILogger<TRequest> logger,
            IAuthService authService,
            IIdentityService identityService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _authService = authService;
            _identityService = identityService;
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
                var claimId = _authService.ClaimID;
                var identifier = await _identityService.GetIdentifier(claimId);

                _logger.LogWarning(
                    $"Application Long Running Request: {requestName} " +
                    $"({elapsedMilliseconds} milliseconds) {claimId} - {identifier}" +
                    $"{request}");
            }

            return response;
        }
    }
}
