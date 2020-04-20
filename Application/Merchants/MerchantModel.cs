using Application.Common.Mappings;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Models
{
    public class MerchantModel : IMapFrom<Merchant>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CallToAction { get; set; }
        public string ShortDescription { get; set; }
        public string Phone { get; set; }
        public string OperatingHours { get; set; }
        public string ContactEmail { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public string WebsiteUrl { get; set; }
        public int MerchantTypeID { get; set; }
        public bool Active { get; set; }
        public string MerchantTypeName { get; set; }
        public List<ItemType> ItemTypes { get; set; }
    }
}
