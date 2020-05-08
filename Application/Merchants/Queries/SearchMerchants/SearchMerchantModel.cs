using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Merchants.Queries.SearchMerchants
{
    public class SearchMerchantModel : IMapFrom<Merchant>
    {
        public SearchMerchantModel()
        {
            ItemTypes = new List<SearchMerchantItemTypeModel>();
            Locations = new List<SearchMerchantLocationModel>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CallToAction { get; set; }
        public string ShortDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int MerchantTypeID { get; set; }
        public string MerchantTypeName { get; set; }
        public List<SearchMerchantItemTypeModel> ItemTypes { get; set; }
        public List<SearchMerchantLocationModel> Locations { get; set; }
        public string DefaultImageUrl { get; set; }
    }
}
