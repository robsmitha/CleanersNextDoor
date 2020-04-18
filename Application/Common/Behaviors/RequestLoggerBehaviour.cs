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
        private readonly IIdentityService _identity;

        public RequestLoggerBehaviour(
            ILogger<TRequest> logger,
            IIdentityService identity)
        {
            _logger = logger;
            _identity = identity;
        }

        public async Task Process(
            TRequest request,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var claimId = _identity.ClaimID;
            var identifier = _identity.UniqueIdentifier;

            _logger.LogInformation($"Application Request: {requestName} {claimId} {identifier} {request}");
            await Task.FromResult(0);
        }
    }
}
