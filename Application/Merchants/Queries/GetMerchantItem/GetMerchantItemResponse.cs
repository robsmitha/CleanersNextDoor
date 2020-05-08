using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantItem
{
    public class GetMerchantItemResponse
    {
        public GetMerchantItemResponse(GetMerchantItemModel item = null)
        {
            Item = item ?? new GetMerchantItemModel();
        }
        public GetMerchantItemModel Item { get; set; }
    }
}
