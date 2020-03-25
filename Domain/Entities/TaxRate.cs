using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaxRate : BaseEntity
    {
        /// <summary>
        /// For percentage based discounts like sales tax
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// For a flat tax like recycling redemption fee
        /// </summary>
        public decimal? TaxAmount { get; set; }
        /// <summary>
        /// ['VAT_TAXABLE' or 'VAT_NON_TAXABLE' or 'VAT_EXEMPT' or 'INTERNAL_TAX']
        /// </summary>
        public int TaxTypeID { get; set; }
        public bool IsDefault { get; set; }

        [ForeignKey("TaxTypeID")]
        public TaxType TaxType { get; set; }
    }
}
