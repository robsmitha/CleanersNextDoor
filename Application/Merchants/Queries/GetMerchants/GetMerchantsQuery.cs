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
    public class GetMerchantsQuery : IRequest<List<MerchantModel>>
    {
        public GetMerchantsQuery()
        {
                
        }
    }

    public class GetMerchantsQueryHandler : IRequestHandler<GetMerchantsQuery, List<MerchantModel>>
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
        public async Task<List<MerchantModel>> Handle(GetMerchantsQuery request, CancellationToken cancellationToken)
        {
            var data = from m in _context.Merchants.AsEnumerable()
                       join i in _context.Items.AsEnumerable() on m.ID equals i.MerchantID into tmp_i
                       from i in tmp_i.DefaultIfEmpty()
                       join it in _context.ItemTypes.AsEnumerable() on i?.ItemTypeID equals it.ID into tmp_it
                       from it in tmp_it.DefaultIfEmpty()
                       where m.Active
                       select new { m, i };

            if (data == null || data.FirstOrDefault() == null) return new List<MerchantModel>();
            var merchants = new Dictionary<int, MerchantModel>();
            foreach (var row in data)
            {
                if (!merchants.TryGetValue(row.m.ID, out var merchant))
                {
                    merchant = _mapper.Map<MerchantModel>(row.m);
                    merchants.Add(row.m.ID, merchant);
                }
                if(row.i != null)
                {
                    merchant.ItemTypes.Add(row.i.ItemType.Name);
                    merchants[row.m.ID] = merchant;
                }
            }
            await Task.FromResult(0);
            return merchants.Values.ToList();
        }
    }
}
