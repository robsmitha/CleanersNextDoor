using AutoMapper;
using Infrastructure.Data;
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
        public async Task<GetMerchantModel> Handle(GetMerchantQuery request, CancellationToken cancellationToken)
        {
            var data = from m in _context.Merchants.AsEnumerable()
                       join ml in _context.MerchantLocations.AsEnumerable() on m.ID equals ml.MerchantID
                       join ct in _context.CorrespondenceTypes.AsEnumerable() on ml.CorrespondenceTypeID equals ct.ID
                       where m.ID == request.MerchantID
                       select new { m, ml };

            if (data == null || data.FirstOrDefault() == null) return new GetMerchantModel();

            var model = _mapper.Map<GetMerchantModel>(data.First().m);
            var locations = data.Select(row => row.ml).ToArray();
            model.Locations.Add(_mapper.Map<MerchantLocationModel>(locations[0]));
            //TODO: revist grouping strategy to group by BaseAddress.Equals()
            for (int i = 1; i < locations.Length; i++)
                if (!locations[i].Equals(locations[i - 1]))
                    model.Locations.Add(_mapper.Map<MerchantLocationModel>(locations[i]));

            await Task.FromResult(0);
            return model;
        }
    }
}
