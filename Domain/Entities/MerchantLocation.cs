using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class MerchantLocation : BaseAddress
    {
        public int MerchantID { get; set; }

        [ForeignKey("MerchantID")]
        public Merchant Merchant { get; set; }
        public bool IsDefault { get; set; }

        /// <summary>
        /// The type of correspondence the address should default for (i.e. Merchant dropoff/pickup)
        /// </summary>
        public int CorrespondenceTypeID { get; set; }
        [ForeignKey("CorrespondenceTypeID")]
        public CorrespondenceType CorrespondenceType { get; set; }

        public string Phone { get; set; }
        public string OperatingHours { get; set; }
        public string ContactEmail { get; set; }
    }
}
