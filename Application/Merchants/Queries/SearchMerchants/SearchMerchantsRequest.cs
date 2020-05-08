using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.SearchMerchants
{
    public class SearchMerchantsRequest
    {
        public string keyword { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public int? miles { get; set; }
        public string location { get; set; }
    }
}
