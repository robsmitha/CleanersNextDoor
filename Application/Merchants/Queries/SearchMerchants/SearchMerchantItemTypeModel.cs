using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.SearchMerchants
{
    public class SearchMerchantItemTypeModel : IMapFrom<ItemType>
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
