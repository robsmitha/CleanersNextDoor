using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants
{
    public class MerchantItemModel : IMapFrom<Item>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ItemTypeID { get; set; }
        public int MerchantID { get; set; }
        public decimal? Price { get; set; }
        public string DisplayPrice => Price != null ? Price.Value.ToString("C") : 0.ToString("C"); 
        public int PriceTypeID { get; set; }
        public int? MaxAllowed { get; set; }
    }
}
