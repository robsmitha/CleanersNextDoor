using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Merchants.Queries.GetMerchant;
using Application.Merchants.Queries.GetMerchantItem;
using Application.Merchants.Queries.GetMerchantItems;
using Application.Merchants.Queries.GetMerchantWorkflow;
using Application.Merchants.Queries.SearchMerchants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppUserService _user;

        public MerchantsController(IMediator mediator, IAppUserService user)
        {
            _mediator = mediator;
            _user = user;
        }

        [HttpPost("SearchMerchants")]
        public async Task<ActionResult<SearchMerchantsResponse>> SearchMerchants(SearchMerchantsRequest request = null)
        {
            return await _mediator.Send(new SearchMerchantsQuery(request));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMerchantModel>> GetMerchant(int id)
        {
            return await _mediator.Send(new GetMerchantQuery(id));
        }
        [HttpGet("{id}/items/{itemTypeId}")]
        public async Task<ActionResult<GetMerchantItemsResponse>> GetMerchantItems(int id, int itemTypeId = 0)
        {
            return await _mediator.Send(new GetMerchantItemsQuery(id, itemTypeId));
        }
        [HttpGet("GetMerchantWorkflow/{id}")]
        public async Task<ActionResult<GetMerchantWorkflowResponse>> GetMerchantWorkflow(int id)
        {
            return await _mediator.Send(new GetMerchantWorkflowQuery(id, _user.ClaimID));
        }
        [HttpGet("GetItem/{id}")]
        public async Task<ActionResult<GetMerchantItemResponse>> GetItem(int id)
        {
            return await _mediator.Send(new GetMerchantItemQuery(id));
        }
    }
}