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
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public string Code { get; set; }
        public string Sku { get; set; }
        public bool DefaultTaxRates { get; set; }
        public bool IsRevenue { get; set; }
        public string LookupCode { get; set; }
        public decimal? Percentage { get; set; }
        public int? MaxAllowed { get; set; }
        public string ShortDescription { get; set; }

        public int MerchantID { get; set; }

        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }

        public int ItemTypeID { get; set; }

        [ForeignKey("ItemTypeID")]
        public ItemType ItemType { get; set; }

        public int UnitTypeID { get; set; }

        [ForeignKey("UnitTypeID")]
        public UnitType UnitType { get; set; }

        public int PriceTypeID { get; set; }

        [ForeignKey("PriceTypeID")]
        public PriceType PriceType { get; set; }
    }
    public static class ItemExtensions
    {
        public static bool HasKeyword(this Item @this, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return true;

            if (@this.ItemType == null) 
                return @this.Name.Contains(keyword) 
                    || @this.Description.Contains(keyword);

            return @this.Name.Contains(keyword) 
                || @this.Description.Contains(keyword)
                || @this.ItemType.Name.Contains(keyword)
                || @this.ItemType.Description.Contains(keyword);
        }
    }
}
