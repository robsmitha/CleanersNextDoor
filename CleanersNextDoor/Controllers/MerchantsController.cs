using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Merchants;
using Application.Merchants.Queries.GetMerchant;
using Application.Merchants.Queries.GetMerchantItems;
using Application.Merchants.Queries.GetMerchants;
using Application.Merchants.Queries.GetMerchantWorkflow;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identity;

        public MerchantsController(IMediator mediator, IIdentityService identity)
        {
            _mediator = mediator;
            _identity = identity;
        }

        [HttpGet("GetMerchants")]
        public async Task<IEnumerable<GetMerchantsModel>> GetMerchants()
        {
            return await _mediator.Send(new GetMerchantsQuery());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMerchantModel>> GetMerchant(int id)
        {
            return await _mediator.Send(new GetMerchantQuery(id));
        }
        [HttpGet("{id}/items")]
        public async Task<IEnumerable<MerchantItemModel>> GetMerchantItems(int id)
        {
            return await _mediator.Send(new GetMerchantItemsQuery(id));
        }
        [HttpGet("GetMerchantWorkflow/{id}")]
        public async Task<ActionResult<GetMerchantWorkflowModel>> GetMerchantWorkflow(int id)
        {
            return await _mediator.Send(new GetMerchantWorkflowQuery(id, _identity.ClaimID));
        }
    }
}