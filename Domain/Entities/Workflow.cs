using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Workflow : BaseType
    {
        /// <summary>
        /// Associated Merchant
        /// </summary>
        public int MerchantID { get; set; }
        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }
    }
}
