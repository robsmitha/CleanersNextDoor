using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Merchants.Queries.GetMerchant
{
    public class MerchantImageModel : IMapFrom<MerchantImage>
    {
        public int ID { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
    }
}