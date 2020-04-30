using Application.Common.Mappings;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Merchants.Queries.GetMerchants
{
    public class GetMerchantsModel : IMapFrom<Merchant>
    {
        public GetMerchantsModel()
        {
            ItemTypes = new HashSet<string>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CallToAction { get; set; }
        public string ShortDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int MerchantTypeID { get; set; }
        public bool Active { get; set; }
        public string MerchantTypeName { get; set; }
        public HashSet<string> ItemTypes { get; set; }
    }
}
