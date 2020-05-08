using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Merchants.Queries.SearchMerchants
{
    public class SearchMerchantsQuery : IRequest<SearchMerchantsResponse>
    {
        public string Keyword { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double Miles { get; set; }
        public string Location { get; set; }

        public SearchMerchantsQuery(SearchMerchantsRequest request)
        {
            if (request == null) return;
            Keyword = request.keyword;
            Latitude = request.lat;
            Longitude = request.lng;
            Miles = request.miles ?? int.MaxValue;
            Location = request.location;
        }
    }

    public class SearchMerchantsQueryHandler : IRequestHandler<SearchMerchantsQuery, SearchMerchantsResponse>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;
        private IGoogleGeocodeService _geocode;

        public SearchMerchantsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IGoogleGeocodeService geocode
            )
        {
            _context = context;
            _mapper = mapper;
            _geocode = geocode;
        }

        public async Task<SearchMerchantsResponse> Handle(SearchMerchantsQuery request, CancellationToken cancellationToken)
        {
            Result location = null;
            if (request.Latitude != null && request.Longitude != null)
                location = await _geocode.ReverseGeocode(request.Latitude.Value, request.Longitude.Value);
            else if (!string.IsNullOrWhiteSpace(request.Location))
                location = await _geocode.Geocode(request.Location);

            if (location != null)
            {
                request.Location = location?.formatted_address;
                request.Latitude = location?.geometry.location.lat ?? 0;
                request.Longitude = location?.geometry.location.lng ?? 0;
            }

            var data = from m in _context.Merchants.AsEnumerable()
                       join mt in _context.MerchantTypes.AsEnumerable() on m.MerchantTypeID equals mt.ID
                       join i in _context.Items.AsEnumerable() on m.ID equals i.MerchantID into tmp_i
                       from i in tmp_i.DefaultIfEmpty()
                       join it in _context.ItemTypes.AsEnumerable() on i?.ItemTypeID equals it.ID into tmp_it
                       from it in tmp_it.DefaultIfEmpty()
                       join ml in _context.MerchantLocations.AsEnumerable() on m.ID equals ml.MerchantID into tmp_ml
                       from ml in tmp_ml.DefaultIfEmpty()
                       join mi in _context.MerchantImages.AsEnumerable() on new { MerchantID = m.ID, IsDefault = true} equals new { mi.MerchantID, mi.IsDefault } into tmp_mi
                       from mi in tmp_mi.DefaultIfEmpty()
                       where ml.WithinMiles(request.Latitude, request.Longitude, request.Miles)
                       select new { m, i, ml, mi };

            if (data == null || data.FirstOrDefault() == null) return new SearchMerchantsResponse(displayLocation: request.Location);
            
            var dict_merchants = new Dictionary<int, SearchMerchantModel>();
            var hs_locations = new HashSet<Tuple<int, int>>();
            var hs_itemTypes = new HashSet<Tuple<int, int>>();

            foreach (var row in data)
            {
                if (!dict_merchants.TryGetValue(row.m.ID, out var merchant))
                {
                    merchant = _mapper.Map<SearchMerchantModel>(row.m);
                    dict_merchants.Add(row.m.ID, merchant);
                }

                if(row.mi != null && !string.IsNullOrEmpty(row.mi.ImageUrl) 
                    && string.IsNullOrEmpty(merchant.DefaultImageUrl))
                    merchant.DefaultImageUrl = row.mi.ImageUrl;

                //handle locations
                if (row.ml != null && !hs_locations.Contains(Tuple.Create(merchant.ID, row.ml.ID)) 
                    && !merchant.Locations.Any(a => a.Street1 == row.ml.Street1))
                {
                    hs_locations.Add(Tuple.Create(merchant.ID, row.ml.ID));

                    var merchantLocation = _mapper.Map<SearchMerchantLocationModel>(row.ml);
                    merchantLocation.MilesAway = request.Latitude != null && request.Longitude != null
                        ? row.ml.HaversineDistance(request.Latitude.Value, request.Longitude.Value)
                        : (double?)null;

                    merchant.Locations.Add(merchantLocation);
                    dict_merchants[row.m.ID] = merchant;
                }

                //handle items
                if (row.i != null && !hs_itemTypes.Contains(Tuple.Create(merchant.ID, row.i.ItemTypeID)))
                {
                    hs_itemTypes.Add(Tuple.Create(merchant.ID, row.i.ItemTypeID));
                    merchant.ItemTypes.Add(_mapper.Map<SearchMerchantItemTypeModel>(row.i.ItemType));
                    dict_merchants[row.m.ID] = merchant;
                }
            }

            List<SearchMerchantModel> merchants;
            if (string.IsNullOrEmpty(request.Keyword))
            {
                merchants = dict_merchants.Values.ToList();
            }
            else
            {
                merchants = dict_merchants.Values
                    .Where(m => m.ItemTypes.Any(i => i.Name.ToLower().Contains(request.Keyword.ToLower())))
                    .ToList();
            }


            return await Task.FromResult(new SearchMerchantsResponse(displayLocation: request.Location, merchants: merchants));
        }
    }
}
