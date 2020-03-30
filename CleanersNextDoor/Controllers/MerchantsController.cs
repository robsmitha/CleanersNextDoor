using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Merchants;
using Application.Merchants.Queries.GetMerchant;
using Application.Merchants.Queries.GetMerchantItems;
using Application.Merchants.Queries.GetMerchants;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetMerchants")]
        public async Task<IEnumerable<MerchantModel>> GetMerchants()
        {
            return await _mediator.Send(new GetMerchantsQuery());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantModel>> GetMerchant(int id)
        {
            return await _mediator.Send(new GetMerchantQuery(id));
        }
        [HttpGet("{id}/items")]
        public async Task<IEnumerable<MerchantItemModel>> GetMerchantItems(int id)
        {
            return await _mediator.Send(new GetMerchantItemsQuery(id));
        }
    }
}