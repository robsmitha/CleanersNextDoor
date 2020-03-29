using AutoMapper;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Merchants.Queries.GetMerchantItems
{
    public class GetMerchantItemsQuery : IRequest<IEnumerable<MerchantItemModel>>
    {
        public int MerchantID { get; set; }
        public GetMerchantItemsQuery(int merchantId)
        {
            MerchantID = merchantId;
        }
    }
    public class GetItemsByMerchantIDQueryHandler : IRequestHandler<GetMerchantItemsQuery, IEnumerable<MerchantItemModel>>
    {
        private readonly ICleanersNextDoorContext _context;
        private readonly IMapper _mapper;

        public GetItemsByMerchantIDQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MerchantItemModel>> Handle(GetMerchantItemsQuery request, CancellationToken cancellationToken)
        {
            var collection = await _context.Items
                .Where(i => i.MerchantID == request.MerchantID)
                .ToListAsync();

            return collection != null
                ? _mapper.Map<IEnumerable<MerchantItemModel>>(collection)
                : new List<MerchantItemModel>();
        }
    }
}
