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
        public bool SelfBoardingApplication { get; set; }
        public bool IsBillable { get; set; }
        [ForeignKey("MerchantTypeID")]
        public MerchantType MerchantType { get; set; }
    }
}
