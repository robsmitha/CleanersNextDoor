using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Merchants.Queries.GetMerchant;
using Application.Merchants.Queries.GetMerchants;
using Application.Models;
using Application.Users.Queries.GetUserByUsername;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    }
}