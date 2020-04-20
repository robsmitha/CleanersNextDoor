using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //TODO: revisit linq query
            var data = await _context.Merchants.ToListAsync();
            var merchants = _mapper.Map<IEnumerable<MerchantModel>>(data);
            foreach (var merchant in merchants)
            {
                merchant.ItemTypes = new List<ItemType>();
                var items = _context.Items.Include(i => i.ItemType).Where(i => i.MerchantID == merchant.ID);
                foreach(var item in items)
                    if (!merchant.ItemTypes.Contains(item.ItemType))
                        merchant.ItemTypes.Add(item.ItemType);
            }
            return merchants;
        }
    }
}
