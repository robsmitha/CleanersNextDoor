using Application.Models;
using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Merchants.Queries.GetMerchants
{
    public class GetMerchantsQuery : IRequest<IEnumerable<MerchantModel>>
    {
        public GetMerchantsQuery()
        {
                
        }
    }

    public class GetMerchantsQueryHandler : IRequestHandler<GetMerchantsQuery, IEnumerable<MerchantModel>>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetMerchantsQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MerchantModel>> Handle(GetMerchantsQuery request, CancellationToken cancellationToken)
        {
            var collection = await _context.Merchants.ToListAsync();
            return collection != null
                ? _mapper.Map<IEnumerable<MerchantModel>>(collection)
                : new List<MerchantModel>();
        }
    }
}
