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

namespace Application.Merchants.Queries.GetMerchantItem
{
    public class GetMerchantItemQuery : IRequest<GetMerchantItemResponse>
    {
        public int ItemID { get; set; }
        public GetMerchantItemQuery(int itemId)
        {
            ItemID = itemId;
        }
        public class GetMerchantItemQueryHandler : IRequestHandler<GetMerchantItemQuery, GetMerchantItemResponse>
        {

            private readonly ICleanersNextDoorContext _context;
            private IMapper _mapper;

            public GetMerchantItemQueryHandler(
                ICleanersNextDoorContext context,
                IMapper mapper
                )
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<GetMerchantItemResponse> Handle(GetMerchantItemQuery request, CancellationToken cancellationToken)
            {
                var data = from i in _context.Items.AsEnumerable()
                           join m in _context.Merchants.AsEnumerable() on i.MerchantID equals m.ID
                           join it in _context.ItemTypes.AsEnumerable() on i.ItemTypeID equals it.ID
                           join ut in _context.UnitTypes.AsEnumerable() on i.UnitTypeID equals ut.ID
                           join pt in _context.PriceTypes.AsEnumerable() on i.PriceTypeID equals pt.ID
                           join ii in _context.ItemImages.AsUntrackedEnumerable() on i.ID equals ii.ItemID into tmp_ii
                           from ii in tmp_ii.DefaultIfEmpty()
                           where i.ID == request.ItemID
                           //TODO: pricing model checks
                           select new { i, ii };

                var rows = data?.ToList();
                if (rows == null || !rows.Any()) return new GetMerchantItemResponse();

                var response = new GetMerchantItemResponse(_mapper.Map<GetMerchantItemModel>(rows.First().i));

                var images = rows
                    .Where(r => r.ii != null)
                    .Select(r => r.ii);

                if (images.Any())
                    response.Item.Images = _mapper.Map<List<GetMerchantItemImageModel>>(images);

                return await Task.FromResult(response);
            }
        }
    }
}
