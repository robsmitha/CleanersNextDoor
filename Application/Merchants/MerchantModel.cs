using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Models
{
    public class MerchantModel : IMapFrom<Merchant>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public int MerchantTypeID { get; set; }
        public bool Active { get; set; }
        public string MerchantTypeName { get; set; }
    }
}
