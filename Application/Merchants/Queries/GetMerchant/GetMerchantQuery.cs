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

namespace Application.Merchants.Queries.GetMerchant
{
    public class GetMerchantQuery : IRequest<MerchantModel>
    {
        public int MerchantID { get;set; }
        public GetMerchantQuery(int merchantId)
        {
            MerchantID = merchantId;
        }
    }

    public class GetMerchantQueryHandler : IRequestHandler<GetMerchantQuery, MerchantModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetMerchantQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MerchantModel> Handle(GetMerchantQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Merchants.FindAsync(request.MerchantID);
            return entity != null
                ? _mapper.Map<MerchantModel>(entity)
                : new MerchantModel();
        }
    }
}
