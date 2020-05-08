using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class MerchantImage : BaseEntity
    {
        public int MerchantID { get; set; }
        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDefault { get; set; }
    }
}
