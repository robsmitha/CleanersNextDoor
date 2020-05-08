using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Merchants.Queries.GetMerchant
{
    public class GetMerchantQuery : IRequest<GetMerchantModel>
    {
        public int MerchantID { get;set; }
        public GetMerchantQuery(int merchantId)
        {
            MerchantID = merchantId;
        }
    }

    public class GetMerchantQueryHandler : IRequestHandler<GetMerchantQuery, GetMerchantModel>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetMerchantQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetMerchantModel> Handle(GetMerchantQuery request, CancellationToken cancellationToken)
        {
            var data = from m in _context.Merchants.AsEnumerable()
                       join mt in _context.MerchantTypes.AsEnumerable() on m.MerchantTypeID equals mt.ID
                       join ml in _context.MerchantLocations.AsEnumerable() on m.ID equals ml.MerchantID into tmp_ml
                       from ml in tmp_ml.DefaultIfEmpty()
                       join ct in _context.CorrespondenceTypes.AsEnumerable() on ml?.CorrespondenceTypeID equals ct.ID into tmp_ct
                       from ct in tmp_ct.DefaultIfEmpty()
                       join mi in _context.MerchantImages.AsEnumerable() on m.ID = m.ID equals mi.MerchantID into tmp_mi
                       from mi in tmp_mi.DefaultIfEmpty()
                       where m.ID == request.MerchantID
                       select new { m, ml, mi };

            if (data == null || data.FirstOrDefault() == null) return new GetMerchantModel();


            var model = _mapper.Map<GetMerchantModel>(data.First().m);
            var dict_ml = new Dictionary<int, MerchantLocationModel>();
            var dict_mi = new Dictionary<int, MerchantImageModel>();
            foreach (var row in data)
            {
                if(row.ml != null && !dict_ml.ContainsKey(row.ml.ID) 
                    && !model.Locations.Any(l => l.City.ToLower().Contains(row.ml.City.ToLower())))
                {
                    dict_ml.Add(row.ml.ID, _mapper.Map<MerchantLocationModel>(row.ml));
                    model.Locations.Add(dict_ml[row.ml.ID]);
                }
                if (row.mi != null  && !dict_mi.ContainsKey(row.mi.ID))
                {
                    dict_mi.Add(row.mi.ID, _mapper.Map<MerchantImageModel>(row.mi));
                    model.Images.Add(dict_mi[row.mi.ID]);
                }
            }

            return await Task.FromResult(model);
        }
    }
}
