using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantItem
{
    public class GetMerchantItemImageModel : IMapFrom<ItemImage>
    {
        public int ID { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
    }
}
