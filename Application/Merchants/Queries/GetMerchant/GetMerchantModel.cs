using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.GetMerchant
{
    public class GetMerchantModel : IMapFrom<Merchant>
    {
        public GetMerchantModel()
        {
            Locations = new List<MerchantLocationModel>();
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
        public List<MerchantLocationModel> Locations { get; set; }
    }
}
