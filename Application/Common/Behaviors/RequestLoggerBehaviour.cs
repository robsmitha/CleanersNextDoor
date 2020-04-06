using Infrastructure.Identity;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class RequestLoggerBehaviour<TRequest>
       : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly IAuthService _authService;
        private readonly IIdentityService _identityService;

        public RequestLoggerBehaviour(
            ILogger<TRequest> logger,
            IAuthService authService,
            IIdentityService identityService)
        {
            _logger = logger;
            _authService = authService;
            _identityService = identityService;
        }

        public async Task Process(
            TRequest request,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var claimId = _authService.ClaimID;
            var identifier = await _identityService.GetIdentifier(claimId);

            _logger.LogInformation($"Application Request: {requestName} {claimId} {identifier} {request}");
        }
    }
}
