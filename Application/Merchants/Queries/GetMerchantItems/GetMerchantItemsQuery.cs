using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Merchants.Queries.GetMerchantItems
{
    public class GetMerchantItemsQuery : IRequest<GetMerchantItemsResponse>
    {
        public int MerchantID { get; set; }
        public int ItemTypeID { get; set; }
        public GetMerchantItemsQuery(int merchantId, int itemTypeId = 0)
        {
            MerchantID = merchantId;
            ItemTypeID = itemTypeId;
        }
    }
    public class GetItemsByMerchantIDQueryHandler : IRequestHandler<GetMerchantItemsQuery, GetMerchantItemsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetItemsByMerchantIDQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetMerchantItemsResponse> Handle(GetMerchantItemsQuery request, CancellationToken cancellationToken)
        {
            var data = from i in _context.Items.AsEnumerable()
                       join it in _context.ItemTypes.AsEnumerable() on i.ItemTypeID equals it.ID
                       join ut in _context.UnitTypes.AsEnumerable() on i.UnitTypeID equals ut.ID
                       join pt in _context.PriceTypes.AsEnumerable() on i.PriceTypeID equals pt.ID
                       join ii in _context.ItemImages.AsEnumerable() on new { ItemID = i.ID, IsDefault = true } equals new { ii.ItemID , ii.IsDefault } into tmp_ii
                       from ii in tmp_ii.DefaultIfEmpty()
                       where i.MerchantID == request.MerchantID
                       select new { i, ii };

            if (data == null || data.FirstOrDefault() == null) return new GetMerchantItemsResponse();
            var items = new List<GetMerchantItemModel>();
            foreach (var row in data)
            {
                var item = _mapper.Map<GetMerchantItemModel>(row.i);
                item.DefaultImageUrl = !string.IsNullOrWhiteSpace(row.ii?.ImageUrl)
                     ? row.ii.ImageUrl
                     : string.Empty; //TODO: configure default image at item type lvl or appsetting
                items.Add(item);
            }
            var response = new GetMerchantItemsResponse();

            if(request.ItemTypeID > 0)
                response.Items = items.Where(i => i.ItemTypeID == request.ItemTypeID).ToList();
            else
                response.Items = items;

            return await Task.FromResult(response);
        }
    }
}
