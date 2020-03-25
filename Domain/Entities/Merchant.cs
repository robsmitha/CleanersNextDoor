using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Merchant : BaseType
    {
        public string WebsiteUrl { get; set; }
        public int MerchantTypeID { get; set; }
        public bool SelfBoardingApplication { get; set; }
        public bool IsBillable { get; set; }
        [ForeignKey("MerchantTypeID")]
        public MerchantType MerchantType { get; set; }
    }
}
