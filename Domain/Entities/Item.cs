using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item : BaseType
    {
        public int ItemTypeID { get; set; }
        public int MerchantID { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public int PriceTypeID { get; set; }
        public int UnitTypeID { get; set; }
        public string Code { get; set; }
        public string Sku { get; set; }
        public bool DefaultTaxRates { get; set; }
        public bool IsRevenue { get; set; }
        public string LookupCode { get; set; }
        public decimal? Percentage { get; set; }

        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }

        [ForeignKey("ItemTypeID")]
        public ItemType ItemType { get; set; }

        [ForeignKey("UnitTypeID")]
        public UnitType UnitType { get; set; }

        [ForeignKey("PriceTypeID")]
        public PriceType PriceType { get; set; }
    }
}
