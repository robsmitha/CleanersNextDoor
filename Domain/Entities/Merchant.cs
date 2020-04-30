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
        public string CallToAction { get; set; }
        public string ShortDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int MerchantTypeID { get; set; }
        [ForeignKey("MerchantTypeID")]
        public MerchantType MerchantType { get; set; }
    }
}
