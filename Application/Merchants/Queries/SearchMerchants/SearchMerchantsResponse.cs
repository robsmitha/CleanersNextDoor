using Application.Common.Mappings;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Merchants.Queries.SearchMerchants
{
    public class SearchMerchantsResponse
    {
        public SearchMerchantsResponse(string displayLocation, List<SearchMerchantModel> merchants = null)
        {
            DisplayLocation = displayLocation;
            Merchants = merchants ?? new List<SearchMerchantModel>();
        }
        public string DisplayLocation { get; set; }
        public List<SearchMerchantModel> Merchants { get; set; }
    }
}
